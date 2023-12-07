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
                this.terminologyPollProcessingService.RetrieveAllTerminologyPolls();

            TerminologyPoll retrievedTerminologyPoll = terminologyPolls.First();
            await this.terminologyPollProcessingService.AddTerminologyPollAsync(retrievedTerminologyPoll);

            string relativeUrl = ...; // Construct the URL based on resourceType and current date
            OntologyAssets retrievedOntologyAssets = 
                await this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(relativeUrl);

            foreach (var asset in retrievedOntologyAssets.Assets)
            {
                TerminologyArtifact terminologyArtifact = ...; // Create a TerminologyArtifact from the asset
                await this.terminologyArtifactProcessingService.
                    ModifyOrAddTerminologyArtifactAsync(terminologyArtifact);
            }
        }
    }
}