// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Audits;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Downloads
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
                var exceptions = new List<Exception>();

                List<Document> retrievedDocuments =
                await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

                foreach (var document in retrievedDocuments)
                {
                    try
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
                                  DecryptedFileName = 
                                    $"/decrypted{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",
                                  Decrypted = false,
                                  CreatedDate = currentDateTime,
                              };

                            Document newBlobDocument = new Document
                            {
                                DocumentData = retrievedDocument.DocumentData,
                                FileName = newIngestionTracking.EncryptedFileName
                            };

                            await this.documentService.AddDocumentAsync(newBlobDocument);
                            await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                            LogAudit(document, currentDateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        Console.WriteLine($"Unable to land document: {document.FileName}");
                        exceptions.Add(ex);
                    }
                }

                if(exceptions.Any())
                {
                    throw new AggregateException($"Unable to land {exceptions.Count} document(s)", exceptions);
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
