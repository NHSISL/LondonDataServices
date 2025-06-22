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
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService : ITerminologyPollService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public TerminologyPollService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<TerminologyPoll> AddTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                TerminologyPoll terminologyPollWithAddAuditApplied = await ApplyAddTerminologyPollAsync(terminologyPoll);
                await ValidateTerminologyPollOnAddAsync(terminologyPollWithAddAuditApplied);

                return await this.storageBroker.InsertTerminologyPollAsync(terminologyPoll);
            });

        public ValueTask<IQueryable<TerminologyPoll>> RetrieveAllTerminologyPollsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllTerminologyPollsAsync());

        public ValueTask<TerminologyPoll> RetrieveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId);

                TerminologyPoll maybeTerminologyPoll = await this.storageBroker
                    .SelectTerminologyPollByIdAsync(terminologyPollId);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPollId);

                return maybeTerminologyPoll;
            });

        public ValueTask<TerminologyPoll> ModifyTerminologyPollAsync(TerminologyPoll terminologyPoll) =>
            TryCatch(async () =>
            {
                TerminologyPoll terminologyPollWithModifyAuditApplied = 
                    await ApplyModifyTerminologyPollAsync(terminologyPoll);

                await ValidateTerminologyPollOnModifyAsync(terminologyPollWithModifyAuditApplied);

                TerminologyPoll maybeTerminologyPoll =
                    await this.storageBroker.SelectTerminologyPollByIdAsync(terminologyPoll.Id);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPoll.Id);

                TerminologyPoll terminologyPollWithModifyAuditAppliedEnsured =
                    await EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                        terminologyPollWithModifyAuditApplied,
                        maybeTerminologyPoll);

                ValidateAgainstStorageTerminologyPollOnModify(
                    inputTerminologyPoll: terminologyPoll,
                    storageTerminologyPoll: maybeTerminologyPoll);

                return await this.storageBroker.UpdateTerminologyPollAsync(terminologyPoll);
            });

        public ValueTask<TerminologyPoll> RemoveTerminologyPollByIdAsync(Guid terminologyPollId) =>
            TryCatch(async () =>
            {
                ValidateTerminologyPollId(terminologyPollId: terminologyPollId);

                TerminologyPoll maybeTerminologyPoll = await this.storageBroker
                    .SelectTerminologyPollByIdAsync(terminologyPollId);

                ValidateStorageTerminologyPoll(maybeTerminologyPoll, terminologyPollId);

                TerminologyPoll terminologyPollWithDeleteAuditApplied =
                    await ApplyDeleteAuditAsync(maybeTerminologyPoll);

                TerminologyPoll updatedTerminologyPoll =
                    await this.storageBroker.UpdateTerminologyPollAsync(terminologyPollWithDeleteAuditApplied);

                await ValidateAgainstStorageTerminologyPollOnDeleteAsync(
                    terminologyPoll: updatedTerminologyPoll,
                    maybeTerminologyPoll: terminologyPollWithDeleteAuditApplied);

                return await this.storageBroker.DeleteTerminologyPollAsync(updatedTerminologyPoll);
            });

        virtual internal async ValueTask<TerminologyPoll> ApplyAddTerminologyPollAsync(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            terminologyPoll.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            terminologyPoll.CreatedDate = auditDateTimeOffset;
            terminologyPoll.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            terminologyPoll.UpdatedDate = auditDateTimeOffset;

            return terminologyPoll;
        }

        virtual internal async ValueTask<TerminologyPoll> ApplyModifyTerminologyPollAsync(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            terminologyPoll.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            terminologyPoll.UpdatedDate = auditDateTimeOffset;

            return terminologyPoll;
        }

        virtual internal async ValueTask<TerminologyPoll> ApplyDeleteAuditAsync(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            terminologyPoll.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            terminologyPoll.UpdatedDate = auditDateTimeOffset;

            return terminologyPoll;
        }

        virtual internal async ValueTask<TerminologyPoll> EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
            TerminologyPoll terminologyPoll,
            TerminologyPoll maybeTerminologyPoll)
        {
            terminologyPoll.CreatedDate = maybeTerminologyPoll.CreatedDate;
            terminologyPoll.CreatedBy = maybeTerminologyPoll.CreatedBy;

            return terminologyPoll;
        }
    }
}