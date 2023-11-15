// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Orchestrations.TerminologyDetails
{
    internal partial class TerminologyDetailsOrchestration : ITerminologyDetailsOrchestrationService
    {
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly IOntologyService ontologyService;
        private readonly IDocumentService documentService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyDetailsOrchestration(
            ITerminologyArtifactService terminologyArtifactService,
            IOntologyService ontologyService,
            IDocumentService documentService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyArtifactService = terminologyArtifactService;
            this.ontologyService = ontologyService;
            this.documentService = documentService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<string> RetrieveArtifactDetailsAsync() =>
            throw new NotImplementedException();
    }
}