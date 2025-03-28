// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingService : ITerminologyArtifactProcessingService
    {
        private readonly ITerminologyArtifactService terminologyArtifactService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TerminologyArtifactProcessingService(
            ITerminologyArtifactService terminologyArtifactService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.terminologyArtifactService = terminologyArtifactService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }
        public ValueTask<IQueryable<TerminologyArtifact>> RetrieveAllTerminologyArtifactsAsync() =>
            TryCatch(async () => await this.terminologyArtifactService.RetrieveAllTerminologyArtifactsAsync());

        public ValueTask<TerminologyArtifact> RetrieveTerminologyArtifactByIdAsync(Guid Id) =>
            TryCatch(async () =>
            {
                ValidateId(Id);

                return await this.terminologyArtifactService.RetrieveTerminologyArtifactByIdAsync(Id);
            });

        public async ValueTask<TerminologyArtifact> ModifyOrAddTerminologyArtifactAsync(
            TerminologyArtifact terminologyArtifact) =>
            await TryCatch(async () =>
            {
                ValidateTerminologyArtifact(terminologyArtifact);
                ValidateId(terminologyArtifact.Id);

                var retrievedTerminologyArtifacts =
                    await this.terminologyArtifactService.RetrieveAllTerminologyArtifactsAsync();

                var maybeTerminologyArtifact = retrievedTerminologyArtifacts
                    .FirstOrDefault(artifact => artifact.FullUrl == terminologyArtifact.FullUrl);

                if (maybeTerminologyArtifact != null)
                {
                    // NB: ONLY update the fields that are coming in from the terminology server.
                    // We do not want to lose the existing statuses in the database i.e. IsCore
                    maybeTerminologyArtifact.ResourceType = terminologyArtifact.ResourceType;
                    maybeTerminologyArtifact.Version = terminologyArtifact.Version;
                    maybeTerminologyArtifact.Name = terminologyArtifact.Name;
                    maybeTerminologyArtifact.Title = terminologyArtifact.Title;
                    maybeTerminologyArtifact.Status = terminologyArtifact.Status;
                    maybeTerminologyArtifact.IsDownloaded = terminologyArtifact.IsDownloaded;
                    maybeTerminologyArtifact.LastUpdated = terminologyArtifact.LastUpdated;
                    maybeTerminologyArtifact.IsError = terminologyArtifact.IsError;
                    maybeTerminologyArtifact.ErrorMessage = terminologyArtifact.ErrorMessage;

                    // TODO:  Remove below once security broker has been added to all the foundation services
                    maybeTerminologyArtifact.UpdatedDate = await this.dateTimeBroker
                        .GetCurrentDateTimeOffsetAsync();

                    return await this.terminologyArtifactService
                        .ModifyTerminologyArtifactAsync(maybeTerminologyArtifact);
                }
                else
                {
                    return await this.terminologyArtifactService
                        .AddTerminologyArtifactAsync(terminologyArtifact);
                }
            });

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id) =>
            TryCatch(async () =>
            {
                ValidateId(Id);

                return await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(Id);
            });

        public ValueTask<TerminologyArtifact?> GetNonDownloadedArtifactAsync() =>
            TryCatch(async () =>
            {
                IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
                     await this.terminologyArtifactService.RetrieveAllTerminologyArtifactsAsync();

                TerminologyArtifact? nonDownloadedArtifact =
                    retrievedTerminologyArtifacts
                        .OrderBy(terminologyArtifact => terminologyArtifact.ResourceType)
                        .FirstOrDefault(terminologyArtifact =>
                            terminologyArtifact.IsCore == true
                            && terminologyArtifact.IsDownloaded == false
                            && terminologyArtifact.IsError == false);

                return await ValueTask.FromResult(nonDownloadedArtifact);
            });
    }
}
