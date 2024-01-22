// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService : ITerminologyMetadataOrchestrationService
    {
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly TerminologyMetadataConfiguration terminologyMetadataConfiguration;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;


        internal TerminologyMetadataOrchestrationService(
            ITerminologyPollProcessingService terminologyPollProcessingService,
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            TerminologyMetadataConfiguration terminologyMetadataConfiguration,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.terminologyPollProcessingService = terminologyPollProcessingService;
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.terminologyMetadataConfiguration = terminologyMetadataConfiguration;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask RetrieveArtifactMetadataAsync(string resourceType) =>
            TryCatch(async () =>
            {
                ValidateResourceType(resourceType);

                TerminologyPoll retrievedTerminologyPoll =
                    await this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(resourceType);

                string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
                ValidateResourceURL(relativeUrl);
                relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
                relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll.ToString());

                DateTimeOffset currentDateTimeOffset =
                    this.dateTimeBroker.GetCurrentDateTimeOffset();

                await ProcessArtifacts(relativeUrl);
                retrievedTerminologyPoll.LastPoll = currentDateTimeOffset;
                await this.terminologyPollProcessingService.ModifyTerminologyPollAsync(retrievedTerminologyPoll);
            });

        private async ValueTask ProcessArtifacts(string relativeUrl)
        {
            OntologyAssets retrievedOntologyAssets =
                await this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(relativeUrl);

            foreach (var asset in retrievedOntologyAssets.Assets)
            {
                DateTimeOffset newDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

                TerminologyArtifact terminologyArtifact = new TerminologyArtifact
                {
                    Id = this.identifierBroker.GetIdentifier(),
                    FullUrl = asset.FullUrl,
                    ResourceType = asset.ResourceType,
                    Version = asset.Version,
                    Name = asset.Name,
                    Title = asset.Title,
                    Status = asset.Status,
                    LastUpdated = asset.LastUpdated,
                    IsCore = false,
                    IsDownloaded = false,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    UpdatedDate = newDateTimeOffset,
                    CreatedDate = newDateTimeOffset
                };

                await this.terminologyArtifactProcessingService
                    .ModifyOrAddTerminologyArtifactAsync(terminologyArtifact);
            }

            if (!string.IsNullOrWhiteSpace(retrievedOntologyAssets.NextPage))
            {
                await ProcessArtifacts(retrievedOntologyAssets.NextPage);
            }
        }
    }
}