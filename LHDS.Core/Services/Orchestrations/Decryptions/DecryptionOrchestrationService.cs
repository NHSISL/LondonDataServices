// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Audits;
using LHDS.Core.Models.Foundations.Documents;
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

        public ValueTask DecryptAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateFileNameIsNotNull(fileName);

                var ingestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByIdAsync(fileName);

                Document document = await this.documentService
                     .RetrieveDocumentByFileNameAsync(ingestionTracking.EncryptedFileName);

                byte[] decryptedData = await this.decryptionService.DecryptAsync(document.DocumentData);

                Document newDecryptedDocument = new Document
                {
                    DocumentData = decryptedData,
                    FileName = ingestionTracking.DecryptedFileName
                };

                await this.documentService.AddDocumentAsync(newDecryptedDocument);

                ingestionTracking.Decrypted = true;

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(ingestionTracking);

                LogAudit(document);
            });

        private void LogAudit(Document document)
        {
            var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

            Audit newAudit =
                new Audit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = document.FileName,
                    Message = $"Decrypted document - {document.FileName}",
                    CreatedDate = currentDateTime
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
