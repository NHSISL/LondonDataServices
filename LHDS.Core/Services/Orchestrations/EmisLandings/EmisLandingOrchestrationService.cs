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
using LHDS.Core.Models.Processings.Documents.Exceptions;
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

        public ValueTask<List<string>> ProcessAsync(SubscriberCredential subscriberCredential, Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateSubscriberCredentials(subscriberCredential);
                ValidateProcessArguments(supplierId);
                var exceptions = new List<Exception>();
                Download download = new Download { SubscriberCredential = subscriberCredential };

                List<string> retrievedDownloadList =
                    await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(download);

                List<string> files = new List<string>();

                foreach (var fileName in retrievedDownloadList)
                {
                    try
                    {
                        string decryptedFile = await TryCatch(async () =>
                        {
                            IngestionTracking? maybeIngestionTracking =
                                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                                    .FirstOrDefault(ingestionTracking =>
                                        ingestionTracking.FileName == fileName);

                            if (maybeIngestionTracking == null)
                            {
                                Download fileToRetrieve = new Download
                                {
                                    Document = new Document { FileName = fileName },
                                    SubscriberCredential = subscriberCredential
                                };

                                Download retrievedDownload =
                                    await this.downloadProcessingService
                                        .RetrieveDownloadByFileNameAsync(fileToRetrieve);

                                string encryptedFileSha256Hash =
                                    this.hashBroker.GenerateSha256Hash(
                                        retrievedDownload.Document?.DocumentData ?? Array.Empty<byte>());

                                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                                DataSetSpecification? retrievedDataSetSpecification = await
                                    this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                                        supplierId);

                                if (retrievedDataSetSpecification == null)
                                {
                                    throw new NotFoundDocumentProcessingException(
                                        $"No active dataset specification found for supplier id: " +
                                        $"{landingConfiguration.LandingSupplierId}");
                                }

                                var filename = fileName.StartsWith('/')
                                    ? fileName
                                    : "/" + fileName;

                                string[] splitFileName = filename.Split('/');
                                string newFileName = "";

                                if (splitFileName.Length < 6)
                                {
                                    throw new InvalidDocumentProcessingFileNameException(filename);
                                }
                                else
                                {
                                    newFileName = $"{subscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
                                }

                                var encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newFileName}";

                                var decryptedFileName =
                                    $"/{landingConfiguration.DecryptedFolder}" +
                                    $"/{retrievedDataSetSpecification?.DataSet?.DataSetName}" +
                                    $"/{retrievedDataSetSpecification?.Id}" +
                                    $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}" +
                                    $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                                IngestionTracking newIngestionTracking =
                                  new IngestionTracking
                                  {
                                      Id = this.identifierBroker.GetIdentifier(),
                                      FileName = fileName,
                                      SupplierId = landingConfiguration.LandingSupplierId,
                                      EncryptedFileName = encryptedFileName,
                                      DecryptedFileName = decryptedFileName,
                                      Decrypted = false,
                                      LastSeen = currentDateTime,
                                      FileDeleted = false,
                                      RecordCount = 0,
                                      EncryptedFileSize = retrievedDownload?.Document?.DocumentData?.Length ?? 0,
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
                                    DocumentData = retrievedDownload?.Document?.DocumentData ?? Array.Empty<byte>(),
                                    FileName = newIngestionTracking.EncryptedFileName
                                };

                                await this.documentProcessingService
                                    .AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                                LogAudit(newIngestionTracking, "Landed");

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

                            return string.Empty;
                        });

                        if (!string.IsNullOrWhiteSpace(decryptedFile))
                        {
                            files.Add(decryptedFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.loggingBroker.LogError(ex);
                        Console.WriteLine($"Unable to land document: {fileName}");
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

        public async ValueTask<string> ProcessFileAsync(
            string ftpFileName,
            SubscriberCredential subscriberCredential,
            Guid supplierId) =>
            await TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateSubscriberCredentials(subscriberCredential);
                ValidateProcessFileArguments(ftpFileName, supplierId);

                Download download = new Download
                {
                    Document = new Document { FileName = ftpFileName },
                    SubscriberCredential = subscriberCredential
                };

                Download externalDownload =
                    await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(download);

                ValidateStorageDownload(externalDownload, ftpFileName);

                IngestionTracking? maybeIngestionTracking =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == ftpFileName);

                if (maybeIngestionTracking != null)
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    maybeIngestionTracking.UpdatedDate = currentDateTime;
                    maybeIngestionTracking.LastSeen = currentDateTime;
                    maybeIngestionTracking.EncryptedFileSize = externalDownload.Document?.DocumentData?.Length ?? 0;

                    try
                    {
                        await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                            maybeIngestionTracking.EncryptedFileName, blobContainers.EmisLanding);
                    }
                    catch (DocumentProcessingDependencyException documentProcessingDependencyException)
                        when (documentProcessingDependencyException.InnerException is FailedDocumentRequestException
                            && documentProcessingDependencyException.InnerException.InnerException.Message
                                .StartsWith("The specified blob does not exist.")
                        )
                    { }

                    Document newBlobDocument = new Document
                    {
                        DocumentData = externalDownload.Document?.DocumentData ?? Array.Empty<byte>(),
                        FileName = maybeIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);
                    await this.documentProcessingService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);

                    LogAudit(
                        ingestionTracking: maybeIngestionTracking,
                        message: "Refreshed");

                    return maybeIngestionTracking.DecryptedFileName;
                }
                else
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    string externalFileName = externalDownload.Document?.FileName ?? string.Empty;

                    var filename = externalFileName.StartsWith('/')
                        ? externalFileName
                        : "/" + externalFileName;

                    string encryptedFileSha256Hash =
                        this.hashBroker.GenerateSha256Hash(externalDownload.Document?.DocumentData ?? []);

                    DataSetSpecification? retrievedDataSetSpecification = await
                        this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                            supplierId);

                    if (retrievedDataSetSpecification == null)
                    {
                        throw new NotFoundDocumentProcessingException(
                            $"No active dataset specification found for supplier id: " +
                            $"{landingConfiguration.LandingSupplierId}");
                    }

                    string[] splitFileName = filename.Split('/');
                    string newFileName = "";

                    if (splitFileName.Length < 6)
                    {
                        throw new InvalidDocumentProcessingFileNameException(filename);
                    }
                    else
                    {
                        newFileName = $"{subscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
                    }

                    var encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newFileName}";

                    if (retrievedDataSetSpecification.DataSet is null)
                    {
                        throw new NotFoundDocumentProcessingException(
                            $"No dataset found for supplier id: " +
                            $"{landingConfiguration.LandingSupplierId}");
                    }

                    var decryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}" +
                        $"/{retrievedDataSetSpecification.DataSet?.DataSetName}" +
                        $"/{retrievedDataSetSpecification.Id}" +
                        $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}" +
                        $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

                    IngestionTracking newIngestionTracking =
                      new IngestionTracking
                      {
                          Id = this.identifierBroker.GetIdentifier(),
                          FileName = externalDownload.Document?.FileName ?? string.Empty,
                          SupplierId = landingConfiguration.LandingSupplierId,
                          EncryptedFileName = encryptedFileName,
                          DecryptedFileName = decryptedFileName,
                          Decrypted = false,
                          LastSeen = currentDateTime,
                          FileDeleted = false,
                          RecordCount = 0,
                          EncryptedFileSize = externalDownload.Document?.DocumentData?.Length ?? 0,
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
                        DocumentData = externalDownload.Document?.DocumentData ?? [],
                        FileName = newIngestionTracking.EncryptedFileName
                    };

                    await this.ingestionTrackingProcessingService.AddIngestionTrackingAsync(newIngestionTracking);
                    await this.documentProcessingService.AddDocumentAsync(newBlobDocument, blobContainers.EmisLanding);
                    LogAudit(newIngestionTracking, "Re-Landed");

                    return newIngestionTracking.DecryptedFileName;
                }
            });

        public ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentials(subscriberCredential);
                Download download = new Download { SubscriberCredential = subscriberCredential };

                List<string> retrievedDownloadList =
                    await this.downloadProcessingService.RetrieveListOfDownloadsToProcessAsync(download);

                return retrievedDownloadList;
            });

        public ValueTask<byte[]> RetrieveDownloadByFileNameAsync(
            string fileName, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentials(subscriberCredential);
                ValidateRetrieveDownloadByFileNameArguments(fileName);

                Download download = new Download
                {
                    Document = new Document { FileName = fileName },
                    SubscriberCredential = subscriberCredential
                };

                Download storageDownload =
                    await this.downloadProcessingService.RetrieveDownloadByFileNameAsync(download);

                return storageDownload.Document?.DocumentData ?? [];
            });

        public ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking retrievedIngestionTracking =
                    await this.ingestionTrackingProcessingService.RetrieveIngestionTrackingByIdAsync(
                        ingestionTrackingId);

                retrievedIngestionTracking.Decrypted = false;
                retrievedIngestionTracking.UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();

                IngestionTracking modifiedIngestionTracking =
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                        retrievedIngestionTracking);
            });

        private void LogAudit(
            IngestionTracking ingestionTracking,
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