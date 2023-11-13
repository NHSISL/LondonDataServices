// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    internal partial class TerminologyArtifactProcessingService : ITerminologyArtifactProcessingService
    {
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyArtifactProcessingService(
            ITerminologyArtifactService terminologyArtifactService,
            ILoggingBroker loggingBroker)
        {
            this.terminologyArtifactService = terminologyArtifactService;
            this.loggingBroker = loggingBroker;
        }
        public IQueryable<TerminologyArtifact> RetrieveAllTerminologyArtifactsAsync() =>
            throw new NotImplementedException();

        public ValueTask<TerminologyArtifact> RetrieveAllTerminologyArtifactByIdAsync(Guid Id) =>
            throw new NotImplementedException();

        public ValueTask<TerminologyArtifact> RetrieveOrAddTerminologyArtifactAsync(
            OntologyAsset ontologyAsset) =>
            throw new NotImplementedException();

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id) =>
            throw new NotImplementedException();
    }
}
