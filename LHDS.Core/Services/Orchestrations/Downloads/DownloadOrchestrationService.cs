// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.Extensions.Configuration;

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
        private readonly IIdentifierBroker identifierBroker;
        private readonly string source;

        public DownloadOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            IIngestionTrackingService ingestionTrackingService,
            IAuditService auditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            IConfiguration configuration)
        {
            this.documentService = documentService;
            this.downloadService = downloadService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.source = configuration["LandingSource"];
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
                        await TryCatch(async () =>
                        {
                            IngestionTracking maybeIngestionTracking =
                                this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                                    .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == document.FileName);

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
                                      Id = this.identifierBroker.GetIdentifier(),
                                      FileName = document.FileName,
                                      Source = source,
                                      EncryptedFileName = $"/encrypted{filename}",
                                      DecryptedFileName =
                                        $"/decrypted{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",
                                      Decrypted = false,
                                      LastSeen = currentDateTime,
                                      FileDeleted = false,
                                      RecordCount = 0,
                                      EncryptedFileSize = retrievedDocument.DocumentData.Length,
                                      DecryptedFileSize = 0,
                                      CreatedBy = "System",
                                      CreatedDate = currentDateTime,
                                      UpdatedBy = "System",
                                      UpdatedDate = currentDateTime
                                  };

                                Document newBlobDocument = new Document
                                {
                                    DocumentData = retrievedDocument.DocumentData,
                                    FileName = newIngestionTracking.EncryptedFileName
                                };

                                await this.documentService.AddDocumentAsync(newBlobDocument);
                                await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                                LogAudit(newIngestionTracking, document, currentDateTime, "Landed");
                            }
                            else
                            {
                                maybeIngestionTracking.LastSeen = dateTimeBroker.GetCurrentDateTimeOffset();
                                await ingestionTrackingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        Console.WriteLine($"Unable to land document: {document.FileName}");
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException($"Unable to land {exceptions.Count} document(s)", exceptions);
                }
            });

        public async ValueTask ProcessAsync(string fileName) =>
            await TryCatch(async () =>
            {
                ValidateFileName(fileName);

                IngestionTracking maybeIngestionTracking =
                    this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                if (maybeIngestionTracking != null)
                {
                    Document externalDocument =
                            await this.downloadService.RetrieveDownloadByFileNameAsync(fileName);

                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    maybeIngestionTracking.LastSeen = currentDateTime;
                    maybeIngestionTracking.EncryptedFileSize = externalDocument.DocumentData.Length;
                    await this.documentService.RemoveDocumentByFileNameAsync(fileName);

                    Document newBlobDocument = new Document
                    {
                        DocumentData = externalDocument.DocumentData,
                        FileName = maybeIngestionTracking.EncryptedFileName
                    };

                    await this.documentService.AddDocumentAsync(newBlobDocument);
                    await this.ingestionTrackingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);

                    LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        document: externalDocument,
                        currentDateTime,
                        message: "Refreshed");
                }
            });

        private void LogAudit(
            IngestionTracking ingestionTracking,
            Document document,
            DateTimeOffset currentDateTime,
            string message)
        {
            Audit newAudit =
                new Audit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"{message} document - {document.FileName}",
                    CreatedBy = "System",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "System",
                    UpdatedDate = currentDateTime
                };

            this.auditService.AddAuditAsync(newAudit);
        }
    }
}
