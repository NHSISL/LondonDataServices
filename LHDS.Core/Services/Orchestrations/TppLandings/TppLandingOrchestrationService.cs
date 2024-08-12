// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
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
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.TppLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppLandingOrchestrationService : ITppLandingOrchestrationService
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

        public TppLandingOrchestrationService(
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

        public async ValueTask<Guid> ProcessAsync(Stream input, string fileName, Guid supplierId) =>
            await TryCatch(async () =>
            {
                ValidateArgumentsOnProcess(input, fileName, supplierId);

                IngestionTracking? maybeIngestionTracking =
                    this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                string decryptedFileSha256Hash =
                    this.hashBroker.GenerateSha256Hash(data: input);

                if (maybeIngestionTracking == null)
                {
                    var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                    DataSetSpecification retrievedDataSetSpecification = await
                        this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(supplierId);

                    var filename = fileName.StartsWith('/')
                        ? fileName
                        : "/" + fileName;

                    string[] segments = filename.Split('/');

                    if (segments.Length < 4)
                    {
                        throw new InvalidOperationException("The input string does not contain enough segments.");
                    }

                    string batch = segments[2];
                    string objectName = Path.GetFileNameWithoutExtension(filename);
                    string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
                    sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");

                    var decryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                        + $"/{retrievedDataSetSpecification?.DataSet?.DataSetName}"
                        + $"/{retrievedDataSetSpecification?.Id}"
                        + $"{filename}";

                    IngestionTracking newIngestionTracking =
                        new IngestionTracking
                        {
                            Id = this.identifierBroker.GetIdentifier(),
                            SupplierId = supplierId,
                            FileName = filename,
                            SourceFolderPath = sourceFolderPath,
                            Batch = batch,
                            ObjectName = objectName,
                            DataSetSpecificationId = retrievedDataSetSpecification.Id,
                            EncryptedFileName = "Not Encrypted",
                            EncryptedFileSize = 0,
                            EncryptedFileSha256Hash = "Not Encrypted",
                            DecryptedFileName = decryptedFileName,
                            Decrypted = true,
                            DecryptedFileSize = 0,
                            DecryptedFileSha256Hash = decryptedFileSha256Hash,
                            FileDeleted = false,
                            RecordCount = 0,
                            CreatedBy = "System",
                            CreatedDate = currentDateTime,
                            UpdatedBy = "System",
                            UpdatedDate = currentDateTime,
                        };

                    await this.ingestionTrackingProcessingService.AddIngestionTrackingAsync(newIngestionTracking);

                    await this.documentProcessingService.AddDocumentAsync(
                        input,
                        fileName: newIngestionTracking.DecryptedFileName,
                        container: blobContainers.Ingress);

                    await LogAudit(newIngestionTracking, "Landed", currentDateTime);

                    return newIngestionTracking.Id;
                }
                else
                {
                    if (maybeIngestionTracking.DecryptedFileSha256Hash == decryptedFileSha256Hash)
                    {
                        return maybeIngestionTracking.Id;
                    }
                    else
                    {
                        var currentDateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                        await this.documentProcessingService.AddDocumentAsync(
                            input,
                            fileName: maybeIngestionTracking.DecryptedFileName,
                            container: blobContainers.Ingress);

                        maybeIngestionTracking.DecryptedFileSha256Hash = decryptedFileSha256Hash;
                        maybeIngestionTracking.UpdatedDate = currentDateTime;

                        await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(
                            maybeIngestionTracking);

                        await LogAudit(
                            maybeIngestionTracking,
                            "Received and updated file from TPP which has now been uploaded to the blob store",
                            currentDateTime);

                        return maybeIngestionTracking.Id;
                    }
                }
            });

        private async ValueTask LogAudit(
           IngestionTracking ingestionTracking,
           string message,
           DateTimeOffset currentDateTime)
        {
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

            await this.ingestionTrackingProcessingAuditService.AddIngestionTrackingAuditAsync(newAudit);
        }
    }
}