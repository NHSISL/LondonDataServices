// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService : IDecryptionOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDownloadService downloadService;
        private readonly ICryptographyService cryptographyService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IHashBroker hashBroker;
        private readonly LandingConfiguration landingConfiguration;

        public DecryptionOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            ICryptographyService cryptographyService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IHashBroker hashBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentService = documentService;
            this.downloadService = downloadService;
            this.cryptographyService = cryptographyService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.hashBroker = hashBroker;
            this.landingConfiguration = landingConfiguration;
        }

        public ValueTask<(string DecryptedFileName, Guid IngestionTrackingId)> DecryptAsync(
            string encryptedFileName,
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateBlobContainersIsNotNull();
                ValidateFileNameIsNotNull(encryptedFileName);
                ValidateSubscriberCredentials(subscriberCredential);

                var ingestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByEncryptedFileNameAsync(encryptedFileName);

                try
                {
                    string batchCompleteFileName =
                        $"{ingestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}".Replace("\\", "/");

                    await this.documentService.RemoveDocumentByFileNameAsync(
                        batchCompleteFileName,
                        this.blobContainers.Ingress);
                }
                catch (Exception)
                { }

                string decryptedFileSha256Hash = string.Empty;
                long fileSize = 0;
                string tempDir = landingConfiguration.TempFilePath;
                string encryptedTempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}_enc.tmp");
                string decryptedTempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}_dec.tmp");

                try
                {
                    using (Stream encryptedDocument = new FileStream(
                        encryptedTempFile,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None))
                    {
                        await this.documentService.RetrieveDocumentByFileNameAsync(
                            output: encryptedDocument,
                            fileName: ingestionTracking?.EncryptedFileName ?? string.Empty,
                            container: blobContainers.EmisLanding);
                    }

                    using (var encryptedTempFileStream = new FileStream(
                        encryptedTempFile,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read))
                    {
                        ValidateStorageDocumentIsNotNull(
                            stream: encryptedTempFileStream,
                            encryptedFileName);
                    }

                    using (Stream encryptedDocument = new FileStream(
                        encryptedTempFile,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read))
                    using (Stream decryptedDocument = new FileStream(
                        decryptedTempFile,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None))
                    {
                        var decryptionStartDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                        await LogAudit(ingestionTracking, $"Decrypting {encryptedFileName}", decryptionStartDateTime);

                        await this.cryptographyService.DecryptAsync(
                             input: encryptedDocument,
                             output: decryptedDocument,
                             subscriberCredential);
                    }

                    using (Stream decryptedDocument = new FileStream(
                        decryptedTempFile,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read))
                    {
                        decryptedFileSha256Hash =
                            await this.hashBroker.GenerateSha256HashAsync(decryptedDocument);

                        fileSize = decryptedDocument.Length;

                        await this.documentService.AddDocumentAsync(
                            input: decryptedDocument,
                            fileName: ingestionTracking.DecryptedFileName,
                            container: blobContainers.Ingress);
                    }
                }
                catch (Exception ex)
                {
                    var errorDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    await LogAudit(ingestionTracking, $"Error Decrypting {encryptedFileName};  " +
                        $"Error: {ex.Message} {ex?.InnerException?.Message}", errorDateTime);

                    throw;
                }
                finally
                {
                    if (File.Exists(encryptedTempFile)) File.Delete(encryptedTempFile);
                    if (File.Exists(decryptedTempFile)) File.Delete(decryptedTempFile);
                }

                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                ingestionTracking.Decrypted = true;
                ingestionTracking.DecryptedFileSize = fileSize;
                ingestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                ingestionTracking.IsProcessing = false;
                ingestionTracking.IsBatchComplete = false;
                ingestionTracking.UpdatedDate = currentDateTime;

                var updatedIngestionTracking = await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                await LogAudit(
                    ingestionTracking,

                    message:
                        $"Decrypted document for Id: {updatedIngestionTracking.Id}, " +
                        $"DecryptedFileSize={updatedIngestionTracking.DecryptedFileSize}, " +
                        $"DecryptedFileSha256Hash={updatedIngestionTracking.DecryptedFileSha256Hash}",

                    currentDateTime);

                return (updatedIngestionTracking.DecryptedFileName, ingestionTracking.Id);
            });

        public ValueTask<string?> GetNextItemToBeDecrypted() =>
            TryCatch(async () =>
            {
                DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

                IngestionTracking item = allIngestionTrackings
                    .OrderBy(ingestionTrackingItem => ingestionTrackingItem.CreatedDate)
                    .FirstOrDefault(ingestionTrackingItem =>
                        ingestionTrackingItem.IsDownloaded == true
                        && ingestionTrackingItem.Decrypted == false
                        && ingestionTrackingItem.IsProcessing == false
                        && ingestionTrackingItem.RetryCount < 4
                        && ingestionTrackingItem.UpdatedDate <
                            currentDateTime.AddMinutes(-landingConfiguration.RelandIntervalInMinutes));

                if (item == null)
                {
                    return null;
                }

                DateTimeOffset currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                item.IsProcessing = true;
                item.RetryCount += 1;
                item.LastAttempt = currentDateTimeOffset;
                item.UpdatedDate = currentDateTimeOffset;
                var modifiedItem = await this.ingestionTrackingService.ModifyIngestionTrackingAsync(item);

                return modifiedItem.EncryptedFileName;
            });

        private async ValueTask LogAudit(
            IngestionTracking ingestionTracking,
            string message,
            DateTimeOffset currentDateTime)
        {
            IngestionTrackingAudit newAudit =
                new IngestionTrackingAudit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"{message}",
                    CreatedDate = currentDateTime,
                    CreatedBy = "DecryptionOrchestrationService",
                    UpdatedDate = currentDateTime,
                    UpdatedBy = "DecryptionOrchestrationService",
                };

            await this.auditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}
