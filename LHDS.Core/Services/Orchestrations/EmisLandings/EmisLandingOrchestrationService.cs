// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Document = LHDS.Core.Models.Foundations.Documents.Document;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class EmisLandingOrchestrationService : IEmisLandingOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IDownloadProcessingService downloadProcessingService;
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly IIngestionTrackingAuditProcessingService auditService;
        private readonly BlobContainers blobContainers;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IHashBroker hashBroker;
        private readonly LandingConfiguration landingConfiguration;

        public EmisLandingOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IDownloadProcessingService downloadProcessingService,
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            IIngestionTrackingAuditProcessingService auditService,
            IDataSetSpecificationProcessingService dataSetSpecificationProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            IHashBroker hashBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentProcessingService = documentProcessingService;
            this.downloadProcessingService = downloadProcessingService;
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.auditService = auditService;
            this.dataSetSpecificationProcessingService = dataSetSpecificationProcessingService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.hashBroker = hashBroker;
            this.landingConfiguration = landingConfiguration;
        }

        public ValueTask<List<string>> ProcessAsync(SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateSubscriberCredentials(subscriberCredential);
                var exceptions = new List<Exception>();
                Download download = new Download { SubscriberCredential = subscriberCredential };

                List<Download> retrievedDownloads =
                    await this.downloadProcessingService.RetrieveListOfDocumentsToProcessAsync(download);

                List<string> files = new List<string>();

                foreach (var downloadItem in retrievedDownloads)
                {
                    try
                    {
                        string decryptedFile = await TryCatch(async () =>
                        {
                            IngestionTracking? maybeIngestionTracking =
                                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                                    .FirstOrDefault(ingestionTracking =>
                                        ingestionTracking.FileName == downloadItem.Document.FileName);

                            if (maybeIngestionTracking == null)
                            {
                                Download retrievedDownload =
                                    await this.downloadProcessingService
                                        .RetrieveDownloadByFileNameAsync(downloadItem);

                                string encryptedFileSha256Hash =
                                    this.hashBroker.GenerateSha256Hash(retrievedDownload.Document.DocumentData);

                                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                                DataSetSpecification retrievedDataSetSpecification = await
                                    this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                                        landingConfiguration.LandingSupplierId);

                                var filename = downloadItem.Document.FileName.StartsWith('/')
                                    ? downloadItem.Document.FileName
                                    : "/" + downloadItem.Document.FileName;

                                IngestionTracking newIngestionTracking =
                                  new IngestionTracking
                                  {
                                      Id = this.identifierBroker.GetIdentifier(),
                                      FileName = downloadItem.Document.FileName,
                                      SupplierId = landingConfiguration.LandingSupplierId,
                                      EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                                      DecryptedFileName =
                                        $"/{landingConfiguration.DecryptedFolder}" +
                                        $"/{retrievedDataSetSpecification.DataSet.DataSetName}" +
                                        $"/{retrievedDataSetSpecification.Id}" +
                                        $"/{filename.Split('_')[3]}" +
                                        $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                                      Decrypted = false,
                                      LastSeen = currentDateTime,
                                      FileDeleted = false,
                                      RecordCount = 0,
                                      EncryptedFileSize = retrievedDownload.Document.DocumentData.Length,
                                      EncryptedFileSha256Hash = encryptedFileSha256Hash,
                                      DecryptedFileSize = 0,
                                      DecryptedFileSha256Hash = string.Empty,
                                      CreatedBy = "System",
                                      CreatedDate = currentDateTime,
                                      UpdatedBy = "System",
                                      UpdatedDate = currentDateTime
                                  };

                                await this.ingestionTrackingProcessingService
                                    .AddIngestionTrackingAsync(newIngestionTracking);

                                Document newBlobDocument = new Document
                                {
                                    DocumentData = retrievedDownload.Document.DocumentData,
                                    FileName = newIngestionTracking.EncryptedFileName
                                };

                                await this.documentProcessingService
                                    .AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                                LogAudit(newIngestionTracking, newBlobDocument, "Landed");

                                return newIngestionTracking.DecryptedFileName;
                            }
                            else
                            {
                                DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                                maybeIngestionTracking.LastSeen = currentDateTime;
                                maybeIngestionTracking.UpdatedDate = currentDateTime;

                                await this.ingestionTrackingProcessingService
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
                        Console.WriteLine($"Unable to land document: {downloadItem.Document.FileName}");
                        exceptions.Add(ex);
                    }
                }

                List<IngestionTracking> unavailableIngestionTrackings =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .Where(ingestionTracking =>
                            ingestionTracking.LastSeen <=
                                this.dateTimeBroker.GetCurrentDateTimeOffset().AddMinutes(-15)).ToList();

                foreach (var item in unavailableIngestionTrackings)
                {
                    item.FileDeleted = true;
                    item.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(item);
                }

                if (exceptions.Any())
                {
                    throw new AggregateException($"Unable to land {exceptions.Count} document(s)", exceptions);
                }

                return files;
            });

        public async ValueTask<string> ProcessFileAsync(string fileName, SubscriberCredential subscriberCredential) =>
            await TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateSubscriberCredentials(subscriberCredential);
                ValidateFileName(fileName);

                Download download = new Download
                {
                    Document = new Document { FileName = fileName },
                    SubscriberCredential = subscriberCredential
                };

                Download externalDownload =
                        await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(download);

                ValidateStorageDownload(externalDownload, fileName);

                IngestionTracking? maybeIngestionTracking =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                if (maybeIngestionTracking != null)
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    maybeIngestionTracking.UpdatedDate = currentDateTime;
                    maybeIngestionTracking.LastSeen = currentDateTime;
                    maybeIngestionTracking.EncryptedFileSize = externalDownload.Document.DocumentData.Length;

                    try
                    {
                        await this.documentProcessingService.RemoveDocumentByFileNameAsync(
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
                        DocumentData = externalDownload.Document.DocumentData,
                        FileName = maybeIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);
                    await this.documentProcessingService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                    LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        document: externalDownload.Document,
                        message: "Refreshed");

                    return maybeIngestionTracking.DecryptedFileName;
                }
                else
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    var filename = externalDownload.Document.FileName.StartsWith('/')
                        ? externalDownload.Document.FileName
                        : "/" + externalDownload.Document.FileName;

                    string encryptedFileSha256Hash =
                        this.hashBroker.GenerateSha256Hash(externalDownload.Document.DocumentData);

                    DataSetSpecification retrievedDataSetSpecification = await
                        this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                            landingConfiguration.LandingSupplierId);

                    IngestionTracking newIngestionTracking =
                      new IngestionTracking
                      {
                          Id = this.identifierBroker.GetIdentifier(),
                          FileName = externalDownload.Document.FileName,
                          SupplierId = landingConfiguration.LandingSupplierId,
                          EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                          DecryptedFileName =
                            $"/{landingConfiguration.DecryptedFolder}" +
                            $"/{retrievedDataSetSpecification.DataSet.DataSetName}" +
                            $"/{retrievedDataSetSpecification.Id}" +
                            $"/{filename.Split('_')[3]}" +
                            $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                          Decrypted = false,
                          LastSeen = currentDateTime,
                          FileDeleted = false,
                          RecordCount = 0,
                          EncryptedFileSize = externalDownload.Document.DocumentData.Length,
                          EncryptedFileSha256Hash = encryptedFileSha256Hash,
                          DecryptedFileSize = 0,
                          DecryptedFileSha256Hash = string.Empty,
                          CreatedBy = "System",
                          CreatedDate = currentDateTime,
                          UpdatedBy = "System",
                          UpdatedDate = currentDateTime
                      };

                    Document newBlobDocument = new Document
                    {
                        DocumentData = externalDownload.Document.DocumentData,
                        FileName = newIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingProcessingService.AddIngestionTrackingAsync(newIngestionTracking);
                    await this.documentProcessingService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);
                    LogAudit(newIngestionTracking, externalDownload.Document, "Re-Landed");

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