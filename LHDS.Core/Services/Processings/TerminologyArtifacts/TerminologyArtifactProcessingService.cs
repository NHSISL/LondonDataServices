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

        public async ValueTask<TerminologyArtifact> ModifyOrAddTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact)
        {
            var maybeTerminologyArtifact =
                await this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(terminologyArtifact.Id);

            if (maybeTerminologyArtifact != null)
            {
                terminologyArtifact.IsDownloaded = false;
                return await this.terminologyArtifactService.ModifyTerminologyArtifactAsync(terminologyArtifact);
            }
            else
            {
                terminologyArtifact.IsCore = false;
                terminologyArtifact.IsDownloaded = false;
                return await this.terminologyArtifactService.AddTerminologyArtifactAsync(terminologyArtifact);
            }
        }

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id) =>
            TryCatch(async () =>
            {
                ValidateId(Id);
                return await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(Id);
            });
    }
}
