// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------


using System;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
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
                Document document = await this.documentService.RetrieveDocumentByFileNameAsync(fileName);
                byte[] decryptedData = await this.decryptionService.DecryptAsync(document.DocumentData);
                document.DocumentData = decryptedData;
                await this.documentService.AddDocumentAsync(document);
                DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                var filename = document.FileName.StartsWith('/')
                           ? document.FileName
                           : "/" + document.FileName;

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = document.FileName,
                      EncryptedFileName = $"/encrypted{filename}",
                      DecryptedFileName = $"/decrypted{filename}",
                      Decrypted = false,
                      CreatedDate = currentDateTime,
                  };


                await this.ingestionTrackingService
                        .RetrieveIngestionTrackingByIdAsync(fileName);

                await this.ingestionTrackingService
                    .ModifyIngestionTrackingAsync(newIngestionTracking);

                LogAudit(document, currentDateTime);
            });

        private void LogAudit(Document document, DateTimeOffset currentDateTime)
        {
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
