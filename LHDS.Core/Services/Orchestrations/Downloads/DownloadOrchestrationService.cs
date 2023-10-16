// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Downloads;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using Document = LHDS.Core.Models.Foundations.Documents.Document;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationService : IDownloadOrchestrationService
    {
        private readonly IDocumentService documentService;
        private readonly IDownloadService downloadService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly BlobContainers blobContainers;
        private readonly ISupplierService supplierService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly Guid supplierId;
        private readonly LandingConfiguration landingConfiguration;

        public DownloadOrchestrationService(
            IDocumentService documentService,
            IDownloadService downloadService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentService = documentService;
            this.downloadService = downloadService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.supplierId = landingConfiguration.LandingSupplierId;
            this.landingConfiguration = landingConfiguration;
        }

        public ValueTask<List<string>> ProcessAsync() =>
            TryCatch(async () =>
            {
                var exceptions = new List<Exception>();

                List<Document> retrievedDocuments =
                    await this.downloadService.RetrieveListOfDocumentsToProcessAsync();

                List<string> files = new List<string>();

                foreach (var document in retrievedDocuments)
                {
                    try
                    {
                        string decryptedFile = await TryCatch(async () =>
                        {
                            IngestionTracking maybeIngestionTracking =
                                this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                                    .FirstOrDefault(ingestionTracking =>
                                        ingestionTracking.FileName == document.FileName);

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
                                      SupplierId = supplierId,
                                      EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                                      DecryptedFileName =
                                        $"/{landingConfiguration.DecryptedFolder}" +
                                        $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

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

                                await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);

                                await this.documentService
                                    .AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                                LogAudit(newIngestionTracking, document, "Landed");

                                return newIngestionTracking.DecryptedFileName;
                            }
                            else
                            {
                                DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                                maybeIngestionTracking.LastSeen = currentDateTime;
                                maybeIngestionTracking.UpdatedDate = currentDateTime;

                                await this.ingestionTrackingService
                                    .ModifyIngestionTrackingAsync(maybeIngestionTracking);
                            }

                            return null;
                        });

                        if (decryptedFile != null)
                        {
                            files.Add(decryptedFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        Console.WriteLine($"Unable to land document: {document.FileName}");
                        exceptions.Add(ex);
                    }
                }

                List<IngestionTracking> unavailableIngestionTrackings =
                    this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                        .Where(ingestionTracking =>
                            ingestionTracking.LastSeen <=
                                this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(-15)).ToList();

                foreach (var item in unavailableIngestionTrackings)
                {
                    item.FileDeleted = true;
                    item.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    await this.ingestionTrackingService.ModifyIngestionTrackingAsync(item);
                }

                if (exceptions.Any())
                {
                    throw new AggregateException($"Unable to land {exceptions.Count} document(s)", exceptions);
                }

                return files;
            });

        public async ValueTask<string> ProcessAsync(string fileName) =>
            await TryCatch(async () =>
            {
                ValidateFileName(fileName);

                Document externalDocument =
                        await this.downloadService.RetrieveDownloadByFileNameAsync(fileName);

                ValidateStorageDownload(externalDocument, fileName);

                IngestionTracking maybeIngestionTracking =
                    this.ingestionTrackingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                if (maybeIngestionTracking != null)
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    maybeIngestionTracking.UpdatedDate = currentDateTime;
                    maybeIngestionTracking.LastSeen = currentDateTime;
                    maybeIngestionTracking.EncryptedFileSize = externalDocument.DocumentData.Length;

                    try
                    {
                        await this.documentService.RemoveDocumentByFileNameAsync(
                            maybeIngestionTracking.EncryptedFileName, blobContainers.EmisLanding);
                    }
                    catch (DocumentDependencyException documentDependencyException)
                        when (documentDependencyException.InnerException is FailedDocumentRequestException
                            && documentDependencyException.InnerException.InnerException.Message
                                .StartsWith("The specified blob does not exist.")
                        )
                    { }

                    Document newBlobDocument = new Document
                    {
                        DocumentData = externalDocument.DocumentData,
                        FileName = maybeIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);
                    await this.documentService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                    LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        document: externalDocument,
                        message: "Refreshed");

                    return maybeIngestionTracking.DecryptedFileName;
                }
                else
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    var filename = externalDocument.FileName.StartsWith('/')
                        ? externalDocument.FileName
                        : "/" + externalDocument.FileName;

                    IngestionTracking newIngestionTracking =
                      new IngestionTracking
                      {
                          Id = this.identifierBroker.GetIdentifier(),
                          FileName = externalDocument.FileName,
                          SupplierId = supplierId,
                          EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                          DecryptedFileName =
                            $"/{landingConfiguration.DecryptedFolder}" +
                            $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                          Decrypted = false,
                          LastSeen = currentDateTime,
                          FileDeleted = false,
                          RecordCount = 0,
                          EncryptedFileSize = externalDocument.DocumentData.Length,
                          DecryptedFileSize = 0,
                          CreatedBy = "System",
                          CreatedDate = currentDateTime,
                          UpdatedBy = "System",
                          UpdatedDate = currentDateTime
                      };

                    Document newBlobDocument = new Document
                    {
                        DocumentData = externalDocument.DocumentData,
                        FileName = newIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingService.AddIngestionTrackingAsync(newIngestionTracking);
                    await this.documentService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);
                    LogAudit(newIngestionTracking, externalDocument, "Re-Landed");

                    return newIngestionTracking.DecryptedFileName;
                }
            });

        private void LogAudit(
            IngestionTracking ingestionTracking,
            Document document,
            string message)
        {
            var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

            IngestionTrackingAudit newAudit =
                new IngestionTrackingAudit
                {
                    Id = Guid.NewGuid(),
                    IngestionTrackingId = ingestionTracking.Id,
                    Message = $"{message} document",
                    CreatedBy = "DownloadOrchestrationService",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "DownloadOrchestrationService",
                    UpdatedDate = currentDateTime
                };

            this.auditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}
