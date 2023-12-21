// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Document = LHDS.Core.Models.Foundations.Documents.Document;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService : ITppOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly IIngestionTrackingAuditProcessingService ingestionTrackingProcessingAuditService;
        private readonly BlobContainers blobContainers;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IHashBroker hashBroker;
        private readonly LandingConfiguration landingConfiguration;

        public TppOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            IIngestionTrackingAuditProcessingService ingestionTrackingProcessingAuditService,
            IDataSetSpecificationProcessingService dataSetSpecificationProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            IHashBroker hashBroker,
            LandingConfiguration landingConfiguration)
        {
            this.documentProcessingService = documentProcessingService;
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.ingestionTrackingProcessingAuditService = ingestionTrackingProcessingAuditService;
            this.dataSetSpecificationProcessingService = dataSetSpecificationProcessingService;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.hashBroker = hashBroker;
            this.landingConfiguration = landingConfiguration;
        }

        public async ValueTask<Guid> ProcessAsync(Document document) =>
            await TryCatch(async () =>
            {
                ValidateDocumentIsNotNull(document);
                ValidateDocumentFileNameIsNotNull(document.FileName);

                IngestionTracking? maybeIngestionTracking =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == document.FileName);

                string encryptedFileSha256Hash =
                    this.hashBroker.GenerateSha256Hash(document.DocumentData);

                var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                if (maybeIngestionTracking == null)
                {
                    DataSetSpecification retrievedDataSetSpecification = await
                        this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(
                            landingConfiguration.LandingSupplierId);

                    var filename = document.FileName.StartsWith('/')
                        ? document.FileName
                        : "/" + document.FileName;

                    IngestionTracking newIngestionTracking =
                        new IngestionTracking
                        {
                            Id = this.identifierBroker.GetIdentifier(),
                            FileName = document.FileName,
                            SupplierId = landingConfiguration.LandingSupplierId,
                            EncryptedFileName = "Not Encrypted",

                            DecryptedFileName =
                                $"/{landingConfiguration.DecryptedFolder}"
                                + $"/{retrievedDataSetSpecification.DataSet.DataSetName}"
                                + $"/{retrievedDataSetSpecification.Id}"
                                + $"/{filename}",

                            Decrypted = true,
                            LastSeen = currentDateTime,
                            FileDeleted = false,
                            RecordCount = 0,
                            EncryptedFileSize = 0,
                            EncryptedFileSha256Hash = "Not Encrypted",
                            DecryptedFileSize = 0,
                            DecryptedFileSha256Hash = encryptedFileSha256Hash,
                            CreatedBy = "System",
                            CreatedDate = currentDateTime,
                            UpdatedBy = "System",
                            UpdatedDate = currentDateTime,
                        };

                    Document newBlobDocument = new Document
                    {
                        DocumentData = document.DocumentData,
                        FileName = newIngestionTracking.DecryptedFileName
                    };

                    await this.ingestionTrackingProcessingService.AddIngestionTrackingAsync(newIngestionTracking);
                    await this.documentProcessingService.AddDocumentAsync(newBlobDocument, blobContainers.Versioner);
                    LogAudit(newIngestionTracking, "Landed");

                    return newIngestionTracking.Id;
                }
                else
                {
                    if (maybeIngestionTracking.DecryptedFileSha256Hash == document.SHA256Hash)
                    {
                        return maybeIngestionTracking.Id;
                    }
                    else
                    {
                        document.SHA256Hash = encryptedFileSha256Hash;
                        await this.documentProcessingService.AddDocumentAsync(document, blobContainers.Versioner);
                        maybeIngestionTracking.DecryptedFileSha256Hash = document.SHA256Hash;
                        maybeIngestionTracking.UpdatedDate = currentDateTime;

                        await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                            maybeIngestionTracking);

                        LogAudit(
                            maybeIngestionTracking,
                            "Received and updated file from TPP which has now been uploaded to the blob store");

                        return maybeIngestionTracking.Id;
                    }
                }
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
                    CreatedBy = "TppOrchestrationService",
                    CreatedDate = currentDateTime,
                    UpdatedBy = "TppOrchestrationService",
                    UpdatedDate = currentDateTime
                };

            this.ingestionTrackingProcessingAuditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}