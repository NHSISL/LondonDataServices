// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Cryptographies;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService : IDecryptionOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly ICryptographyService cryptographyService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IHashBroker hashBroker;

        public DecryptionOrchestrationService(
            IDocumentService documentService,
            ICryptographyService cryptographyService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IHashBroker hashBroker)
        {
            this.documentService = documentService;
            this.cryptographyService = cryptographyService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.hashBroker = hashBroker;
        }

        public ValueTask<string> DecryptAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateBlobContainersIsNotNull();
                ValidateFileNameIsNotNull(fileName);

                var ingestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByFileNameAsync(fileName);

                Document document = await this.documentService
                    .RetrieveDocumentByFileNameAsync(
                        fileName: ingestionTracking.EncryptedFileName,
                        container: blobContainers.EmisLanding);

                byte[] decryptedData = await this.cryptographyService.DecryptAsync(document.DocumentData);

                string decryptedFileSha256Hash =
                    this.hashBroker.GenerateSha256Hash(decryptedData);

                string[] lines = System.Text.Encoding.UTF8.GetString(decryptedData)
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                Document newDecryptedDocument = new Document
                {
                    DocumentData = decryptedData,
                    FileName = ingestionTracking.DecryptedFileName
                };

                await this.documentService.AddDocumentAsync(
                    document: newDecryptedDocument,
                    container: blobContainers.Versioner);

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                ingestionTracking.Decrypted = true;
                ingestionTracking.RecordCount = lines.Length - 2;
                ingestionTracking.DecryptedFileSize = newDecryptedDocument.DocumentData.Length;
                ingestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                ingestionTracking.UpdatedDate = currentDateTime;

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                LogAudit(ingestionTracking, document: newDecryptedDocument, currentDateTime);

                return ingestionTracking.DecryptedFileName;
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
