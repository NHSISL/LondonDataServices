// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
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

        public TerminologyMetadataOrchestrationService(
            ITerminologyPollProcessingService terminologyPollProcessingService,
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            TerminologyMetadataConfiguration terminologyMetadataConfiguration,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyPollProcessingService = terminologyPollProcessingService;
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.terminologyMetadataConfiguration = terminologyMetadataConfiguration;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask RetrieveArtifacMetadataAsync(string resourceType) 
        {
            IQueryable<TerminologyPoll> terminologyPolls =
                this.terminologyPollProcessingService.RetrieveAllTerminologyPolls()
                    .Where(terminologyPoll => terminologyPoll.ResourceType == resourceType);

            TerminologyPoll retrievedTerminologyPoll = terminologyPolls.First();
            await this.terminologyPollProcessingService.AddTerminologyPollAsync(retrievedTerminologyPoll);

            string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;

            DateTimeOffset currentDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

            OntologyAssets retrievedOntologyAssets = 
                await this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(relativeUrl);

            retrievedOntologyAssets.Assets = retrievedOntologyAssets.Assets
                .Where(asset => asset.LastUpdated.HasValue && asset.LastUpdated.Value < currentDateTimeOffset)
                    .ToList();

            foreach (var asset in retrievedOntologyAssets.Assets)
            {
                DateTimeOffset dateTimeOffset= this.dateTimeBroker.GetCurrentDateTimeOffset();

                TerminologyArtifact terminologyArtifact = new TerminologyArtifact
                {
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
                    UpdatedDate = dateTimeOffset,
                    CreatedDate = dateTimeOffset
                };

                await this.terminologyArtifactProcessingService.
                    ModifyOrAddTerminologyArtifactAsync(terminologyArtifact);
            }

            await this.terminologyPollProcessingService.AddTerminologyPollAsync(retrievedTerminologyPoll);
        }
    }
}