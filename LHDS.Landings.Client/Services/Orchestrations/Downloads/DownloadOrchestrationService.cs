// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Foundations.Audits;
using LHDS.Landings.Client.Services.Foundations.Documents;
using LHDS.Landings.Client.Services.Foundations.Downloads;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Services.Orchestrations.Download
{
    public partial class DownloadOrchestrationService : IDownloadOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDownloadService downloadService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IAuditService auditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public DownloadOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            IIngestionTrackingService ingestionTrackingService,
            IAuditService auditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.documentService = documentService;
            this.downloadService = downloadService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask ProcessAsync()
        {
            List<Document> retrievedDocuments =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

            foreach (var document in retrievedDocuments)
            {
                IngestionTracking maybeIngestionTracking =
                    await this.ingestionTrackingService
                        .RetrieveIngestionTrackingByFileNameAsync(document.FileName);

                //if (maybeIngestionTracking != null)
                //{
                Document retrievedDocument =
                    await this.downloadService.RetrieveDownloadByFileNameAsync(document.FileName);

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = document.FileName,
                        FileName = document.FileName,
                        Decrypted = false,
                        CreatedDate = currentDateTime,
                    };

                await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                await this.documentService.AddDocumentAsync(document);
                LogAudit(document, currentDateTime);
                //}
            }
        }

        private void LogAudit(Document document, DateTimeOffset currentDateTime)
        {
            Audit newAudit =
                new Audit
                {
                    IngestionTrackingId = document.FileName,
                    Message = $"Landed document - {document.FileName}",
                    CreatedDate = currentDateTime
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
