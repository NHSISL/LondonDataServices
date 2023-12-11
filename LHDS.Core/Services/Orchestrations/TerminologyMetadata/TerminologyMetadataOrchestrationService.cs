// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Orchestrations.TerminologyMedata;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    internal partial class TerminologyMetadataOrchestrationService : ITerminologyMetadataOrchestrationService
    {
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly TerminologyMetadataConfiguration terminologyMetadataConfiguration;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;


        public TerminologyMetadataOrchestrationService(
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

        public async ValueTask RetrieveArtifactMetadataAsync(string resourceType)
        {
            TerminologyPoll retrievedTerminologyPoll =
                await this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(resourceType);

            string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
            relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll.ToString());
            await ProcessArtifacts(relativeUrl);
            DateTimeOffset currentDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();
            retrievedTerminologyPoll.LastPoll = currentDateTimeOffset;
            await this.terminologyPollProcessingService.ModifyTerminologyPollAsync(retrievedTerminologyPoll);
        }

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