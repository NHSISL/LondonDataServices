// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Document = LHDS.Core.Models.Foundations.Documents.Document;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class EmisLandingOrchestrationService : IEmisLandingOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IDownloadProcessingService downloadProcessingService;
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly IIngestionTrackingAuditProcessingService auditService;
        private readonly BlobContainers blobContainers;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IHashBroker hashBroker;
        private readonly IFileBroker fileBroker;
        private readonly LandingConfiguration landingConfiguration;

        public EmisLandingOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IDownloadProcessingService downloadProcessingService,
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            IIngestionTrackingAuditProcessingService auditService,
            IDataSetSpecificationProcessingService dataSetSpecificationProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            IHashBroker hashBroker,
            IFileBroker fileBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentProcessingService = documentProcessingService;
            this.downloadProcessingService = downloadProcessingService;
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.auditService = auditService;
            this.dataSetSpecificationProcessingService = dataSetSpecificationProcessingService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.hashBroker = hashBroker;
            this.fileBroker = fileBroker;
            this.landingConfiguration = landingConfiguration;
        }

        public ValueTask<List<string>> ProcessAsync(SubscriberCredential subscriberCredential, Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateSubscriberCredentials(subscriberCredential);
                ValidateProcessArguments(supplierId);
                var exceptions = new List<Exception>();
                Download download = new Download { SubscriberCredential = subscriberCredential };

                List<string> retrievedDownloadList =
                    await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(download);

                List<string> files = new List<string>();

                foreach (var fileName in retrievedDownloadList)
                {
                    try
                    {
                        string decryptedFile = await TryCatch(async () =>
                        {
                            return await ProcessFileAsync(subscriberCredential, supplierId, fileName);
                        });

                        if (!string.IsNullOrWhiteSpace(decryptedFile))
                        {
                            files.Add(decryptedFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        await this.loggingBroker.LogErrorAsync(ex);
                        Console.WriteLine($"Unable to download document: {fileName}");
                        exceptions.Add(ex);
                    }
                }

                DateTimeOffset fifteenMinsAgo = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                fifteenMinsAgo.AddMinutes(-15);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

                List<IngestionTracking> unavailableIngestionTrackings = allIngestionTrackings
                        .Where(ingestionTracking =>
                            ingestionTracking.LastSeen <= fifteenMinsAgo).ToList();

                foreach (var item in unavailableIngestionTrackings)
                {
                    item.FileDeleted = true;
                    item.UpdatedDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(item);
                }

                if (exceptions.Any())
                {
                    throw new AggregateException($"Unable to land {exceptions.Count} document(s)", exceptions);
                }

                return files;
            });

        public ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentials(subscriberCredential);
                Download download = new Download { SubscriberCredential = subscriberCredential };

                List<string> retrievedDownloadList =
                    await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(download);

                return retrievedDownloadList;
            });

        public ValueTask RetrieveDownloadByFileNameAsync(
            Stream output,
            string fileName,
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateRetrieveDownloadByFileNameArguments(output, fileName, subscriberCredential);

                Download download = new Download
                {
                    Document = new Document { FileName = fileName, DocumentData = output },
                    SubscriberCredential = subscriberCredential
                };

                await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(download);

                ValidateStorageDownload(output, fileName);
            });

        public ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking retrievedIngestionTracking =
                    await this.ingestionTrackingProcessingService.RetrieveIngestionTrackingByIdAsync(
                        ingestionTrackingId);

                retrievedIngestionTracking.Decrypted = false;
                retrievedIngestionTracking.UpdatedDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                IngestionTracking modifiedIngestionTracking =
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                        retrievedIngestionTracking);
            });

        private async ValueTask<string> ProcessFileAsync(
            SubscriberCredential subscriberCredential,
            Guid supplierId,
            string fileName)
        {
            IQueryable<IngestionTracking> allIngestionTrackings =
                await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                .FirstOrDefault(ingestionTracking =>
                    ingestionTracking.FileName == fileName);

            if (maybeIngestionTracking == null)
            {
                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                var filename = fileName.StartsWith('/')
                    ? fileName
                    : "/" + fileName;

                string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
                sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");
                string file = Path.GetFileName(filename);
                string fileWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string[] segments = fileWithoutExtension.Split('_');
                string batch = $"{segments[0]}_{segments[1]}";
                string objectName = $"{segments[2]}_{segments[3]}";

                DataSetSpecification? retrievedDataSetSpecification = await
                    this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                        supplierId);

                (string encryptedFileName, string decryptedFileName, string baseFolder) =
                        await GetFileNames(subscriberCredential, retrievedDataSetSpecification, filename, supplierId);

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = await this.identifierBroker.GetIdentifierAsync(),
                      SupplierId = supplierId,
                      FileName = filename,
                      SourceFolderPath = sourceFolderPath,
                      BatchReadyFolderPath = baseFolder,
                      Batch = batch,
                      IsBatchComplete = false,
                      ObjectName = objectName,
                      DataSetSpecificationId = retrievedDataSetSpecification.Id,
                      EncryptedFileName = encryptedFileName,
                      DecryptedFileName = decryptedFileName,
                      Decrypted = false,
                      LastSeen = currentDateTime,
                      FileDeleted = false,
                      RecordCount = 0,
                      RetryCount = 0,
                      LastAttempt = currentDateTime,
                      EncryptedFileSize = 0,
                      EncryptedFileSha256Hash = string.Empty,
                      DecryptedFileSize = 0,
                      DecryptedFileSha256Hash = string.Empty,
                      IsDownloaded = false,
                      CreatedBy = "System",
                      CreatedDate = currentDateTime,
                      UpdatedBy = "System",
                      UpdatedDate = currentDateTime
                  };

                maybeIngestionTracking = await this.ingestionTrackingProcessingService
                    .AddIngestionTrackingAsync(newIngestionTracking);

                await LogAudit(maybeIngestionTracking, $"New file found - {fileName}");
            }

            if (maybeIngestionTracking.IsDownloaded == false && maybeIngestionTracking.RetryCount <= 3)
            {
                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                maybeIngestionTracking.RetryCount += 1;
                maybeIngestionTracking.IsDownloaded = false;
                maybeIngestionTracking.IsBatchComplete = false;
                maybeIngestionTracking.FileDeleted = false;
                maybeIngestionTracking.EncryptedFileSize = 0;
                maybeIngestionTracking.EncryptedFileSha256Hash = string.Empty;
                maybeIngestionTracking.LastSeen = currentDateTime;
                maybeIngestionTracking.UpdatedDate = currentDateTime;

                await LogAudit(maybeIngestionTracking, $"Downloading {fileName};  " +
                    $"Attempt(s): {maybeIngestionTracking.RetryCount}");

                IngestionTracking updatedIngestionTracking =
                    await this.ingestionTrackingProcessingService
                        .ModifyIngestionTrackingAsync(maybeIngestionTracking);

                string tempEncryptedFilePath = await this.fileBroker.GetTempFileName();

                try
                {
                    using (FileStream writeFtpFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        await DownloadFile(output: writeFtpFileStream, fileName, subscriberCredential);
                    }

                    using (FileStream readFtpFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        string encryptedFileSha256Hash = await this.hashBroker.GenerateSha256HashAsync(readFtpFileStream);
                        updatedIngestionTracking.EncryptedFileSize = readFtpFileStream.Length;
                        updatedIngestionTracking.EncryptedFileSha256Hash = encryptedFileSha256Hash;

                        await this.documentProcessingService.AddDocumentAsync(
                            input: readFtpFileStream,
                            fileName: maybeIngestionTracking.EncryptedFileName,
                            container: blobContainers.EmisLanding);
                    }
                }
                catch (Exception ex)
                {
                    await LogAudit(updatedIngestionTracking, $"Error Downloading {fileName};  " +
                        $"Error: {ex.Message} {ex?.InnerException?.Message}");

                    throw;
                }
                finally
                {
                    await this.fileBroker.DeleteFileAsync(tempEncryptedFilePath);
                }

                var updatedDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                updatedIngestionTracking.IsDownloaded = true;
                updatedIngestionTracking.Decrypted = false;
                updatedIngestionTracking.IsProcessing = false;
                updatedIngestionTracking.RetryCount = 0;
                updatedIngestionTracking.FileDeleted = false;
                updatedIngestionTracking.LastSeen = currentDateTime;
                updatedIngestionTracking.UpdatedDate = updatedDate;

                await this.ingestionTrackingProcessingService
                    .ModifyIngestionTrackingAsync(updatedIngestionTracking);

                await LogAudit(updatedIngestionTracking, $"Downloaded {fileName};  " +
                    $"Attempt(s): {maybeIngestionTracking.RetryCount}");

                return updatedIngestionTracking.DecryptedFileName;
            }

            return string.Empty;
        }

        private async ValueTask LogAudit(
            IngestionTracking ingestionTracking,
            string message)
        {
            var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            IngestionTrackingAudit newAudit =
                new IngestionTrackingAudit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"{message}",
                    CreatedBy = "DownloadOrchestrationService",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "DownloadOrchestrationService",
                    UpdatedDate = currentDateTime
                };

            await this.auditService.AddIngestionTrackingAuditAsync(newAudit);
        }

        private async ValueTask<(string encryptedFileName, string decryptedFileName, string baseFolder)> GetFileNames(
            SubscriberCredential subscriberCredential,
            DataSetSpecification? retrievedDataSetSpecification,
            string fileName,
            Guid supplierId)
        {
            if (retrievedDataSetSpecification == null)
            {
                throw new NotFoundDocumentProcessingException(
                    $"No active dataset specification found for supplier id: " +
                    $"{supplierId}");
            }

            string[] splitFileName = fileName.Split('/');
            string newFileName = "";

            if (splitFileName.Length < 6)
            {
                throw new InvalidArgumentsDocumentProcessingException(fileName);
            }

            string dataSetName = retrievedDataSetSpecification?.DataSet?.DataSetName ?? string.Empty;
            string dataSetVersion = retrievedDataSetSpecification?.OurSpecificationVersion ?? string.Empty;
            string extractGroup = subscriberCredential.Id.ToString();
            string extractTime = splitFileName[5];

            string baseFolder =
                $"/{landingConfiguration.DecryptedFolder}" +
                $"/{dataSetName}" +
                $"/{dataSetVersion}" +
                $"/{extractGroup}" +
                $"/{extractTime}";

            newFileName = $"{splitFileName[6]}";

            string encryptedFileName =
                $"/{landingConfiguration.EncryptedFolder}" +
                $"/{extractGroup}" +
                $"/{extractTime}" +
                $"/{newFileName}";

            string decryptedFileName =
                $"{baseFolder}" +
                $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            return (encryptedFileName, decryptedFileName, baseFolder);
        }

        private async ValueTask DownloadFile(
            Stream output,
            string fileName,
            SubscriberCredential subscriberCredential)
        {
            Download fileToRetrieve = new Download
            {
                Document = new Document { FileName = fileName, DocumentData = output },
                SubscriberCredential = subscriberCredential
            };

            await this.downloadProcessingService
                .RetrieveDownloadByFileNameAsync(fileToRetrieve);
        }
    }
}