// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Decryptions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService : IDecryptionOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDecryptionService decryptionService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IAuditService auditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public DecryptionOrchestrationService(
            IDocumentService documentService,
            IDecryptionService decryptionService,
            IIngestionTrackingService ingestionTrackingService,
            IAuditService auditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.documentService = documentService;
            this.decryptionService = decryptionService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<string> DecryptAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateFileNameIsNotNull(fileName);

                var ingestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByFileNameAsync(fileName);

                Document document = await this.documentService
                    .RetrieveDocumentByFileNameAsync(ingestionTracking.EncryptedFileName);

                byte[] decryptedData = await this.decryptionService.DecryptAsync(document.DocumentData);

                string[] lines = System.Text.Encoding.UTF8.GetString(decryptedData)
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                Document newDecryptedDocument = new Document
                {
                    DocumentData = decryptedData,
                    FileName = ingestionTracking.DecryptedFileName
                };

                await this.documentService.AddDocumentAsync(newDecryptedDocument);
                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                ingestionTracking.Decrypted = true;
                ingestionTracking.RecordCount = lines.Length - 1;
                ingestionTracking.DecryptedFileSize = newDecryptedDocument.DocumentData.Length;
                ingestionTracking.UpdatedDate = currentDateTime;

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                LogAudit(ingestionTracking, document: newDecryptedDocument, currentDateTime);

                return ingestionTracking.DecryptedFileName;
            });

        private void LogAudit(IngestionTracking ingestionTracking, Document document, DateTimeOffset currentDateTime)
        {
            Audit newAudit =
                new Audit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"Decrypted document",
                    CreatedDate = currentDateTime,
                    CreatedBy = "DecryptionOrchestrationService",
                    UpdatedDate = currentDateTime,
                    UpdatedBy = "DecryptionOrchestrationService",
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
