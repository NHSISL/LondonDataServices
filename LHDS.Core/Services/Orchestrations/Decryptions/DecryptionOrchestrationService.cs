// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Processings.Downloads;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService : IDecryptionOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDownloadProcessingService downloadProcessingService;
        private readonly ICryptographyService cryptographyService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IHashBroker hashBroker;

        public DecryptionOrchestrationService(
            IDocumentService documentService,
            IDownloadProcessingService downloadProcessingService,
            ICryptographyService cryptographyService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IHashBroker hashBroker)
        {
            this.documentService = documentService;
            this.downloadProcessingService = downloadProcessingService;
            this.cryptographyService = cryptographyService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.hashBroker = hashBroker;
        }

        public ValueTask<string> DecryptAsync(string encryptedFileName, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateBlobContainersIsNotNull();
                ValidateFileNameIsNotNull(encryptedFileName);
                ValidateSubscriberCredentials(subscriberCredential);

                var ingestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByEncryptedFileNameAsync(encryptedFileName);

                Document document = await this.documentService
                   .RetrieveDocumentByFileNameAsync(
                       fileName: ingestionTracking.EncryptedFileName,
                       container: blobContainers.EmisLanding);

                ValidateStorageDocumentIsNotNull(document, encryptedFileName);

                byte[] decryptedData = await this.cryptographyService.DecryptAsync(
                    data: document.DocumentData,
                    subscriberCredential);

                string decryptedFileSha256Hash =
                    this.hashBroker.GenerateSha256Hash(decryptedData);

                string[] lines = System.Text.Encoding.UTF8.GetString(decryptedData)
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                Document newDecryptedDocument = new Document
                {
                    DocumentData = decryptedData,
                    FileName = ingestionTracking.DecryptedFileName
                };

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                ingestionTracking.Decrypted = true;
                ingestionTracking.RecordCount = lines.Length - 2;
                ingestionTracking.DecryptedFileSize = newDecryptedDocument.DocumentData.Length;
                ingestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                ingestionTracking.IsProcessing = false;
                ingestionTracking.UpdatedDate = currentDateTime;

                await this.documentService.AddDocumentAsync(
                    document: newDecryptedDocument,
                    container: blobContainers.Versioner);

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                LogAudit(ingestionTracking, document: document, currentDateTime);

                return ingestionTracking.DecryptedFileName;
            });

        public ValueTask<string?> GetNextItemToBeDecrypted() =>
            TryCatch(async () =>
            {
                DateTimeOffset olderThanDateTimeOffset =
                this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(-15);

                var item = this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                    .FirstOrDefault(ingestionTrackingItem =>
                        ingestionTrackingItem.Decrypted == false
                        && ingestionTrackingItem.IsProcessing == false
                        && ingestionTrackingItem.RetryCount < 4
                        && ingestionTrackingItem.UpdatedDate < olderThanDateTimeOffset);

                if (item == null)
                {
                    return null;
                }

                DateTimeOffset currentDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

                item.IsProcessing = true;
                item.RetryCount += 1;
                item.UpdatedDate = currentDateTimeOffset;
                var modifiedItem = await this.ingestionTrackingService.ModifyIngestionTrackingAsync(item);

                return modifiedItem.EncryptedFileName;
            });

        private void LogAudit(IngestionTracking ingestionTracking, Document document, DateTimeOffset currentDateTime)
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

            this.auditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}
