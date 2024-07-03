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
                        this.loggingBroker.LogError(ex);
                        Console.WriteLine($"Unable to download document: {fileName}");
                        exceptions.Add(ex);
                    }
                }

                List<IngestionTracking> unavailableIngestionTrackings =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .Where(ingestionTracking =>
                            ingestionTracking.LastSeen <=
                                this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(-15)).ToList();

                foreach (var item in unavailableIngestionTrackings)
                {
                    item.FileDeleted = true;
                    item.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

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
                retrievedIngestionTracking.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

                IngestionTracking modifiedIngestionTracking =
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                        retrievedIngestionTracking);
            });

        private async ValueTask<string> ProcessFileAsync(
            SubscriberCredential subscriberCredential,
            Guid supplierId,
            string fileName)
        {
            IngestionTracking? maybeIngestionTracking =
                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTracking =>
                        ingestionTracking.FileName == fileName);

            if (maybeIngestionTracking == null)
            {
                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                var filename = fileName.StartsWith('/')
                    ? fileName
                    : "/" + fileName;

                (string encryptedFileName, string decryptedFileName) =
                        await GetFileNames(subscriberCredential, filename, supplierId);

                string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
                sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = this.identifierBroker.GetIdentifier(),
                      FileName = filename,
                      SourceFolderPath = sourceFolderPath,
                      SupplierId = landingConfiguration.LandingSupplierId,
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
                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                maybeIngestionTracking.RetryCount += 1;
                maybeIngestionTracking.IsDownloaded = false;
                maybeIngestionTracking.FileDeleted = false;
                maybeIngestionTracking.EncryptedFileSize = 0;
                maybeIngestionTracking.EncryptedFileSha256Hash = string.Empty;
                maybeIngestionTracking.LastSeen = currentDateTime;
                maybeIngestionTracking.UpdatedDate = currentDateTime;

                await LogAudit(maybeIngestionTracking, $"Downloading {fileName};  " +
                    $"Attempt: {maybeIngestionTracking.RetryCount}");

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
                        string encryptedFileSha256Hash = this.hashBroker.GenerateSha256Hash(readFtpFileStream);
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

                var updatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

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
                    $"Attempt: {updatedIngestionTracking.RetryCount}");

                return updatedIngestionTracking.DecryptedFileName;
            }

            return string.Empty;
        }

        private async ValueTask LogAudit(
            IngestionTracking ingestionTracking,
            string message)
        {
            var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

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

        private async ValueTask<(string encryptedFileName, string decryptedFileName)> GetFileNames(
            SubscriberCredential subscriberCredential,
            string fileName,
            Guid supplierId)
        {
            DataSetSpecification? retrievedDataSetSpecification = await
                this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                    supplierId);

            if (retrievedDataSetSpecification == null)
            {
                throw new NotFoundDocumentProcessingException(
                    $"No active dataset specification found for supplier id: " +
                    $"{landingConfiguration.LandingSupplierId}");
            }

            string[] splitFileName = fileName.Split('/');
            string newFileName = "";

            if (splitFileName.Length < 6)
            {
                throw new InvalidArgumentsDocumentProcessingException(fileName);
            }
            else
            {
                newFileName = $"{subscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
            }

            string encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newFileName}";

            string decryptedFileName = $"/{landingConfiguration.DecryptedFolder}" +
                $"/{retrievedDataSetSpecification?.DataSet?.DataSetName}" +
                $"/{retrievedDataSetSpecification?.Id}" +
                $"/{fileName.Split('_')[2]}_{fileName.Split('_')[3]}" +
                $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            return (encryptedFileName, decryptedFileName);
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