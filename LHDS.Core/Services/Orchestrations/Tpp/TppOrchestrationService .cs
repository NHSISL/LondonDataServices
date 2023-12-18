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
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Downloads;
using LHDS.Core.Services.Processings.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService : ITppOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IDownloadProcessingService downloadProcessingService;
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
            IDownloadProcessingService downloadProcessingService,
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
            this.downloadProcessingService = downloadProcessingService;
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

                if (maybeIngestionTracking != null)
                {
                    return maybeIngestionTracking.Id;
                }
                else
                {
                    document.SHA256Hash = this.hashBroker.GenerateSha256Hash(document.DocumentData);

                    await this.documentProcessingService.AddDocumentAsync(document, blobContainers.TppLanding);
                    maybeIngestionTracking.DecryptedFileSha256Hash = document.SHA256Hash;
                    await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);

                    IngestionTrackingAudit audit = new IngestionTrackingAudit
                    {
                        Id = this.identifierBroker.GetIdentifier(),
                        IngestionTrackingId = maybeIngestionTracking.Id,
                        Message = "Updated TPP Hash"
                    };

                    await this.ingestionTrackingProcessingAuditService.AddIngestionTrackingAuditAsync(audit);

                    return maybeIngestionTracking.Id;
                }
            });
    }
}