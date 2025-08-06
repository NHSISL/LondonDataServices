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
                ValidateOnProcess(subscriberCredential, supplierId);

                var exceptions = new List<Exception>();
                List<string> files = new List<string>();

                try
                {
                    files.AddRange(await ProcessSubscriberFiles(subscriberCredential, supplierId));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                try
                {
                    await MarkItemsAsDeleteThatHasNotBeenSeen(subscriberCredential.Id);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                if (exceptions.Any())
                {
                    throw new AggregateException($"Unable to process documents", exceptions);
                }

                return files;
            });

        virtual internal async ValueTask<List<string>> ProcessSubscriberFiles(
            SubscriberCredential subscriberCredential,
            Guid supplierId)
        {
            Download download = new Download { SubscriberCredential = subscriberCredential };
            List<string> files = new List<string>();
            var exceptions = new List<Exception>();

            List<string> retrievedDownloadList =
                await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(download);

            foreach (var fileName in retrievedDownloadList)
            {
                try
                {
                    string encryptedFile = await TryCatch(async () =>
                    {
                        return await ProcessFileAsync(subscriberCredential, supplierId, fileName);
                    });

                    if (!string.IsNullOrWhiteSpace(encryptedFile))
                    {
                        files.Add(encryptedFile);
                    }
                }
                catch (Exception ex)
                {
                    await this.loggingBroker.LogErrorAsync(ex);
                    Console.WriteLine($"Unable to download document: {fileName}");
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to process ingestion tracking items", exceptions);
            }

            return files;
        }

        public ValueTask CheckForBatchReady(SubscriberCredential subscriberCredential, Guid supplierId) =>
            TryCatch(async () =>
            {
                throw new NotImplementedException();
            });

        virtual internal async ValueTask MarkItemsAsDeleteThatHasNotBeenSeen(Guid SubscriberAgreementId)
        {
            var exceptions = new List<Exception>();
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset lastSeenMinutes = currentDateTime.AddMinutes(-this.landingConfiguration.LastSeenMinutes);

            IQueryable<IngestionTracking> allIngestionTrackings =
                await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            List<IngestionTracking> unavailableIngestionTrackings = allIngestionTrackings
                .Where(ingestionTracking =>
                    ingestionTracking.LastSeen <= lastSeenMinutes
                    && !ingestionTracking.FileDeleted
                    && ingestionTracking.SubscriberAgreementId == SubscriberAgreementId)
                .ToList();

            foreach (var item in unavailableIngestionTrackings)
            {
                try
                {
                    item.FileDeleted = true;
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(item);
                }
                catch (Exception ex)
                {
                    await this.loggingBroker.LogErrorAsync(ex);
                    Console.WriteLine($"Unable to mark ingestion tracking item as deleted with id: {item.Id}");
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to mark {exceptions.Count} ingestion tracking items as deleted", exceptions);
            }
        }

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

        virtual internal async ValueTask<string> ProcessFileAsync(
            SubscriberCredential subscriberCredential,
            Guid supplierId,
            string fileName)
        {
            IQueryable<IngestionTracking> allIngestionTrackings =
                await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                .FirstOrDefault(ingestionTracking =>
                    ingestionTracking.FileName == fileName);

            var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            if (maybeIngestionTracking == null)
            {
                var externalFileName = fileName.StartsWith('/')
                    ? fileName
                    : "/" + fileName;

                string sourceFolderPath = Path.GetDirectoryName(externalFileName) ?? string.Empty;
                sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");
                string file = Path.GetFileName(externalFileName);
                string fileWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string[] segments = fileWithoutExtension.Split('_');
                string batch = $"{segments[0]}_{segments[1]}";
                string objectName = $"{segments[2]}_{segments[3]}";

                DataSetSpecification? retrievedDataSetSpecification = await
                    this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                        supplierId);

                (string encryptedFileName, string decryptedFileName, string baseFolder) =
                        await GetFileNames(
                            subscriberCredential,
                            retrievedDataSetSpecification,
                            externalFileName,
                            supplierId);

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = await this.identifierBroker.GetIdentifierAsync(),
                      SupplierId = supplierId,
                      FileName = externalFileName,
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
                      RetryCount = 0,
                      LastAttempt = currentDateTime,
                      EncryptedFileSize = 0,
                      EncryptedFileSha256Hash = string.Empty,
                      DecryptedFileSize = 0,
                      DecryptedFileSha256Hash = string.Empty,
                      IsDownloaded = false,
                      SubscriberAgreementId = subscriberCredential.Id,
                  };

                var storageIngestionTracking = await this.ingestionTrackingProcessingService
                    .AddIngestionTrackingAsync(newIngestionTracking);

                await LogAudit(
                    ingestionTracking: storageIngestionTracking,
                    message:
                        $"New file found '{storageIngestionTracking.FileName}',  " +
                        $"created item with Id: {storageIngestionTracking.Id}");

                maybeIngestionTracking = storageIngestionTracking;
            }

            if (maybeIngestionTracking.IsDownloaded == false && maybeIngestionTracking.RetryCount < 4)
            {
                maybeIngestionTracking.RetryCount += 1;

                await LogAudit(
                    ingestionTracking: maybeIngestionTracking,
                    message:
                        $"Processing file '{maybeIngestionTracking.FileName}' " +
                        $"associated with Id: {maybeIngestionTracking.Id}." + Environment.NewLine +
                        $"Downloading: {maybeIngestionTracking.FileName} " + Environment.NewLine +
                        $"RetryCount: {maybeIngestionTracking.RetryCount}");

                try
                {
                    string batchReadyFileName =
                        $"{maybeIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                            .Replace("\\", "/");

                    await LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        message:
                            $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                            $"ready status for batch: {maybeIngestionTracking.Batch}.");

                    await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                        batchReadyFileName,
                        this.blobContainers.Ingress);
                }
                catch (Exception)
                { }

                maybeIngestionTracking.IsDownloaded = false;
                maybeIngestionTracking.IsBatchComplete = false;
                maybeIngestionTracking.FileDeleted = false;
                maybeIngestionTracking.EncryptedFileSize = 0;
                maybeIngestionTracking.EncryptedFileSha256Hash = string.Empty;

                await LogAudit(
                    maybeIngestionTracking,
                    $"Downloading {maybeIngestionTracking.FileName};  " +
                        $"RetryCount: {maybeIngestionTracking.RetryCount}");

                IngestionTracking updatedIngestionTracking =
                    await this.ingestionTrackingProcessingService
                        .ModifyIngestionTrackingAsync(maybeIngestionTracking);

                string tempEncryptedFilePath = await this.fileBroker.GetTempFileName();

                try
                {
                    using (FileStream writeFtpFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        Download fileToRetrieve = new Download
                        {
                            Document = new Document
                            {
                                FileName = updatedIngestionTracking.FileName,
                                DocumentData = writeFtpFileStream
                            },

                            SubscriberCredential = subscriberCredential
                        };

                        await this.downloadProcessingService
                            .RetrieveDownloadByFileNameAsync(fileToRetrieve);
                    }

                    using (FileStream readFtpFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        string encryptedFileSha256Hash =
                            await this.hashBroker.GenerateSha256HashAsync(readFtpFileStream);

                        updatedIngestionTracking.EncryptedFileSize = readFtpFileStream.Length;
                        updatedIngestionTracking.EncryptedFileSha256Hash = encryptedFileSha256Hash;

                        await this.documentProcessingService.AddDocumentAsync(
                            input: readFtpFileStream,
                            fileName: updatedIngestionTracking.EncryptedFileName,
                            container: blobContainers.EmisLanding);
                    }

                    await LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        message:
                            $"Downloaded file '{maybeIngestionTracking.FileName}' " +
                            $"and successfully uploaded to blob storage '{maybeIngestionTracking.EncryptedFileName}'");
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

                await this.ingestionTrackingProcessingService
                    .ModifyIngestionTrackingAsync(updatedIngestionTracking);

                await LogAudit(updatedIngestionTracking, $"Updated ingestion tracking info to " +
                    $"reflect successful processing of {updatedIngestionTracking.FileName}");

                return updatedIngestionTracking.DecryptedFileName;
            }

            maybeIngestionTracking = await this.ingestionTrackingProcessingService
                .RetrieveIngestionTrackingByIdAsync(maybeIngestionTracking.Id);

            maybeIngestionTracking.LastSeen = currentDateTime;
            maybeIngestionTracking.FileDeleted = false;

            await this.ingestionTrackingProcessingService
                .ModifyIngestionTrackingAsync(maybeIngestionTracking);

            return string.Empty;
        }

        virtual internal async ValueTask LogAudit(
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
                    CreatedBy = "EmisLandingOrchestrationService",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "EmisLandingOrchestrationService",
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
            string extractTime = splitFileName[6].Split('_')[4];

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
    }
}


