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

namespace LHDS.Landings.Client.Services.Orchestrations.Downloads
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

        public ValueTask ProcessAsync() =>
            TryCatch(async () =>
            {
                List<Document> retrievedDocuments =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

                foreach (var document in retrievedDocuments)
                {
                    IngestionTracking maybeIngestionTracking =
                        await this.ingestionTrackingService
                            .RetrieveIngestionTrackingByIdAsync(document.FileName);

                    if (maybeIngestionTracking == null)
                    {
                        Document retrievedDocument =
                            await this.downloadService.RetrieveDownloadByFileNameAsync(document.FileName);

                        var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

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

                        Document newBlobDocument = new Document
                        {
                            DocumentData = retrievedDocument.DocumentData,
                            FileName = newIngestionTracking.EncryptedFileName
                        };

                        await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                        await this.documentService.AddDocumentAsync(newBlobDocument);
                        LogAudit(document, currentDateTime);
                    }
                }
            });

        private void LogAudit(Document document, DateTimeOffset currentDateTime)
        {
            Audit newAudit =
                new Audit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = document.FileName,
                    Message = $"Landed document - {document.FileName}",
                    CreatedDate = currentDateTime
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
