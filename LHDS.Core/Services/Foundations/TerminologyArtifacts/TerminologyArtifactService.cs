// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService : ITerminologyArtifactService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyArtifactService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TerminologyArtifact> AddTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact) =>
            TryCatch(async () =>
            {
                TerminologyArtifact terminologyArtifactWithAddAuditApplied = 
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(terminologyArtifact);

                await ValidateTerminologyArtifactOnAddAsync(terminologyArtifactWithAddAuditApplied);

                return await this.storageBroker.InsertTerminologyArtifactAsync(terminologyArtifactWithAddAuditApplied);
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
                TerminologyArtifact terminologyArtifactWithModifyAuditApplied = 
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(terminologyArtifact);

                await ValidateTerminologyArtifactOnModifyAsync(terminologyArtifactWithModifyAuditApplied);

                TerminologyArtifact maybeTerminologyArtifact =
                    await this.storageBroker.SelectTerminologyArtifactByIdAsync(
                        terminologyArtifactWithModifyAuditApplied.Id);

                ValidateStorageTerminologyArtifact(maybeTerminologyArtifact, terminologyArtifact.Id);

                TerminologyArtifact terminologyArtifactWithModifyAuditAppliedEnsured =
                    await this.securityAuditBroker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                        terminologyArtifact,
                        maybeTerminologyArtifact);

                ValidateAgainstStorageTerminologyArtifactOnModify(
                    inputTerminologyArtifact: terminologyArtifactWithModifyAuditAppliedEnsured,
                    storageTerminologyArtifact: maybeTerminologyArtifact);

                return await this.storageBroker.UpdateTerminologyArtifactAsync(
                    terminologyArtifactWithModifyAuditAppliedEnsured);
            });

        public ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid terminologyArtifactId) =>
           TryCatch(async () =>
           {
                ValidateTerminologyArtifactId(terminologyArtifactId);

                TerminologyArtifact maybeTerminologyArtifact = 
                    await this.storageBroker.SelectTerminologyArtifactByIdAsync(terminologyArtifactId);

                ValidateStorageTerminologyArtifact(maybeTerminologyArtifact, terminologyArtifactId);

                TerminologyArtifact terminologyArtifactWithDeleteAuditApplied = 
                    await this.securityAuditBroker.ApplyRemoveAuditValuesAsync(maybeTerminologyArtifact);

                TerminologyArtifact updatedTerminologyArtifact =
                    await this.storageBroker.UpdateTerminologyArtifactAsync(terminologyArtifactWithDeleteAuditApplied);

                await ValidateAgainstStorageTerminologyArtifactOnDeleteAsync(
                    updatedTerminologyArtifact,
                    terminologyArtifactWithDeleteAuditApplied);

                return await this.storageBroker.DeleteTerminologyArtifactAsync(updatedTerminologyArtifact);
           });
    }
}