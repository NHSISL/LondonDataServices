// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Audits;
using LHDS.Landings.Client.Services.Foundations.Decryptions;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Services.Orchestrations.Decryptions
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
                    IngestionTrackingId = document.FileName,
                    Message = $"Decrypted document - {document.FileName}",
                    CreatedDate = currentDateTime
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
