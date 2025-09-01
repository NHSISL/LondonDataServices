// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DecisionPolls;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollService : IDecisionPollService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;
        public DecisionPollService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DecisionPoll> AddDecisionPollAsync(DecisionPoll decisionPoll) =>
            TryCatch(async () =>
            {
                decisionPoll = await this.securityAuditBroker.ApplyAddAuditValuesAsync(decisionPoll);
                await ValidateDecisionPollOnAddAsync(decisionPoll);

                return await this.storageBroker.InsertDecisionPollAsync(decisionPoll);
            });

        public ValueTask<DecisionPoll> ModifyDecisionPollAsync(DecisionPoll decisionPoll) =>
            TryCatch(async () =>
            {
                DecisionPoll decisionPollWithModifyAuditApplied = await ApplyModifyDecisionPollAsync(decisionPoll);
                await ValidateDecisionPollOnModifyAsync(decisionPollWithModifyAuditApplied);

                DecisionPoll maybeDecisionPoll =
                    await this.storageBroker.SelectDecisionPollByIdAsync(decisionPoll.Id);

                ValidateStorageDecisionPoll(maybeDecisionPoll, decisionPoll.Id);

                return await this.storageBroker.UpdateDecisionPollAsync(decisionPollWithModifyAuditApplied);
            });

        virtual internal async ValueTask<DecisionPoll> ApplyAddDecisionPollAsync(DecisionPoll decisionPoll)
        {
            ValidateDecisionPollIsNotNull(decisionPoll);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            decisionPoll.CreatedBy = auditUser?.EntraUserId ?? string.Empty;
            decisionPoll.CreatedDate = auditDateTimeOffset;
            decisionPoll.UpdatedBy = auditUser?.EntraUserId ?? string.Empty;
            decisionPoll.UpdatedDate = auditDateTimeOffset;

            return decisionPoll;
        }

        virtual internal async ValueTask<DecisionPoll> ApplyModifyDecisionPollAsync(DecisionPoll decisionPoll)
        {
            ValidateDecisionPollIsNotNull(decisionPoll);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            decisionPoll.UpdatedBy = auditUser?.EntraUserId ?? string.Empty;
            decisionPoll.UpdatedDate = auditDateTimeOffset;

            return decisionPoll;
        }
    }
}
