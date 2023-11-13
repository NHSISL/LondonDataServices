// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingService : ITerminologyArtifactProcessingService
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
            TryCatch(() =>
            {
                return this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();
            });

        public ValueTask<TerminologyArtifact> RetrieveTerminologyArtifactByIdAsync(Guid Id) =>
        TryCatch(async () =>
        {
            ValidateId(Id);
            return await this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(Id);
        });

        public ValueTask<TerminologyArtifact> RetrieveOrAddTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact) =>
                throw new NotImplementedException();

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id) =>
            throw new NotImplementedException();
    }
}
