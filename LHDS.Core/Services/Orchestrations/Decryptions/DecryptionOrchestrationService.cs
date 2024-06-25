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

                long lines = 0;
                string decryptedFileSha256Hash = string.Empty;
                long fileSize = 0;

                using (Stream encryptedDocument = new MemoryStream())
                using (Stream decryptedDocument = new MemoryStream())
                {
                    await this.documentService.RetrieveDocumentByFileNameAsync(
                        output: encryptedDocument,
                        fileName: ingestionTracking?.EncryptedFileName ?? string.Empty,
                        container: blobContainers.EmisLanding);

                    ValidateStorageDocumentIsNotNull(encryptedDocument, encryptedFileName);

                    await this.cryptographyService.DecryptAsync(
                        input: encryptedDocument,
                        output: decryptedDocument,
                        subscriberCredential);

                    decryptedFileSha256Hash =
                        this.hashBroker.GenerateSha256Hash(decryptedDocument);

                    fileSize = decryptedDocument?.Length ?? 0;
                    lines = CountLines(decryptedDocument);

                    await this.documentService.AddDocumentAsync(
                        input: decryptedDocument,
                        fileName: ingestionTracking.DecryptedFileName,
                        container: blobContainers.Versioner);
                }

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                ingestionTracking.Decrypted = true;
                ingestionTracking.RecordCount = lines;
                ingestionTracking.DecryptedFileSize = fileSize;
                ingestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                ingestionTracking.IsProcessing = false;
                ingestionTracking.UpdatedDate = currentDateTime;

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                LogAudit(ingestionTracking, currentDateTime);

                return ingestionTracking.DecryptedFileName;
            });

        static long CountLines(Stream input)
        {
            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            int bytesRead;
            bool lastWasNewLine = true;

            while ((bytesRead = input.Read(buffer, 0, bufferSize)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    if (buffer[i] == '\n')
                    {
                        count++;
                    }
                }
                lastWasNewLine = (buffer[bytesRead - 1] == '\n');
            }

            if (!lastWasNewLine)
            {
                count++;
            }

            return count;
        }

        public ValueTask<string?> GetNextItemToBeDecrypted() =>
            TryCatch(async () =>
            {
                DateTimeOffset olderThanDateTimeOffset =
                this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(-15);

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

                DateTimeOffset currentDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

                item.IsProcessing = true;
                item.RetryCount += 1;
                item.LastAttempt = currentDateTimeOffset;
                item.UpdatedDate = currentDateTimeOffset;
                var modifiedItem = await this.ingestionTrackingService.ModifyIngestionTrackingAsync(item);

                return modifiedItem.EncryptedFileName;
            });

        private void LogAudit(IngestionTracking ingestionTracking, DateTimeOffset currentDateTime)
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
