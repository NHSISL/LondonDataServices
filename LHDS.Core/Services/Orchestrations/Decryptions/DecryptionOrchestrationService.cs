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

        public DecryptionOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            ICryptographyService cryptographyService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IHashBroker hashBroker)
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

                string decryptedFileSha256Hash = string.Empty;
                long fileSize = 0;

                using (Stream encryptedDocument = new MemoryStream())
                using (Stream decryptedDocument = new MemoryStream())
                {
                    await this.documentService.RetrieveDocumentByFileNameAsync(
                        output: encryptedDocument,
                        fileName: ingestionTracking?.EncryptedFileName ?? string.Empty,
                        container: blobContainers.EmisLanding);

                    ValidateStorageDocumentIsNotNull(stream: encryptedDocument, encryptedFileName);

                    await this.cryptographyService.DecryptAsync(
                        input: encryptedDocument,
                        output: decryptedDocument,
                        subscriberCredential);

                    decryptedFileSha256Hash =
                        this.hashBroker.GenerateSha256Hash(decryptedDocument);

                    fileSize = decryptedDocument?.Length ?? 0;

                    await this.documentService.AddDocumentAsync(
                        input: decryptedDocument,
                        fileName: ingestionTracking.DecryptedFileName,
                        container: blobContainers.Ingress);
                }

                var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                ingestionTracking.Decrypted = true;
                ingestionTracking.RecordCount = 0;
                ingestionTracking.DecryptedFileSize = fileSize;
                ingestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                ingestionTracking.IsProcessing = false;
                ingestionTracking.UpdatedDate = currentDateTime;

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                await LogAudit(ingestionTracking, currentDateTime);

                return (ingestionTracking.DecryptedFileName, ingestionTracking.Id);
            });

        public ValueTask<string?> GetNextItemToBeDecrypted() =>
            TryCatch(async () =>
            {
                DateTimeOffset olderThanDateTimeOffset =
                    await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                olderThanDateTimeOffset.AddMinutes(-15);

                var item = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTrackingItem =>
                        ingestionTrackingItem.IsDownloaded == true
                        && ingestionTrackingItem.Decrypted == false
                        && ingestionTrackingItem.IsProcessing == false
                        && ingestionTrackingItem.RetryCount < 4
                        && ingestionTrackingItem.LastAttempt <= olderThanDateTimeOffset);

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
            DateTimeOffset currentDateTime)
        {
            IngestionTrackingAudit newAudit =
                new IngestionTrackingAudit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"Decrypted document",
                    CreatedDate = currentDateTime,
                    CreatedBy = "DecryptionOrchestrationService",
                    UpdatedDate = currentDateTime,
                    UpdatedBy = "DecryptionOrchestrationService",
                };

            await this.auditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}
