// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Orchestrations.Ontologies;
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
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyMetadataOrchestrationService(
            ITerminologyPollProcessingService terminologyPollProcessingService,
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            OntologyConfiguration ontologyConfiguration,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.terminologyPollProcessingService = terminologyPollProcessingService;
            this.ontologyConfiguration = ontologyConfiguration;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask RetrieveArtifacMetadataAsync(string resourceType) =>
            throw new NotImplementedException();
    }
}