// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService : ITerminologyArtifactService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyArtifactService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TerminologyArtifact> AddTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact) =>
            TryCatch(async () =>
            {
                await ValidateTerminologyArtifactOnAddAsync(terminologyArtifact);

                return await this.storageBroker.InsertTerminologyArtifactAsync(terminologyArtifact);
            });

        public ValueTask<IQueryable<TerminologyArtifact>> RetrieveAllTerminologyArtifactsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllTerminologyArtifactsAsync());

        public ValueTask<TerminologyArtifact> RetrieveTerminologyArtifactByIdAsync(Guid terminologyArtifactId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyArtifactId(terminologyArtifactId);

                TerminologyArtifact maybeTerminologyArtifact = await this.storageBroker
                    .SelectTerminologyArtifactByIdAsync(terminologyArtifactId);

                ValidateStorageTerminologyArtifact(maybeTerminologyArtifact, terminologyArtifactId);

                return maybeTerminologyArtifact;
            });

        public ValueTask<TerminologyArtifact> ModifyTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact) =>
            TryCatch(async () =>
            {
                await ValidateTerminologyArtifactOnModifyAsync(terminologyArtifact);

                TerminologyArtifact maybeTerminologyArtifact =
                    await this.storageBroker.SelectTerminologyArtifactByIdAsync(terminologyArtifact.Id);

                ValidateStorageTerminologyArtifact(maybeTerminologyArtifact, terminologyArtifact.Id);

                ValidateAgainstStorageTerminologyArtifactOnModify(
                    inputTerminologyArtifact: terminologyArtifact,
                    storageTerminologyArtifact: maybeTerminologyArtifact);

                return await this.storageBroker.UpdateTerminologyArtifactAsync(terminologyArtifact);
            });

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid terminologyArtifactId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyArtifactId(terminologyArtifactId);

                TerminologyArtifact maybeTerminologyArtifact = await this.storageBroker
                    .SelectTerminologyArtifactByIdAsync(terminologyArtifactId);

                ValidateStorageTerminologyArtifact(maybeTerminologyArtifact, terminologyArtifactId);

                return await this.storageBroker.DeleteTerminologyArtifactAsync(maybeTerminologyArtifact);
            });
    }
}