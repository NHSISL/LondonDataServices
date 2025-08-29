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
        private readonly ILoggingBroker loggingBroker;
        public DecisionPollService(
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

        public async ValueTask<DecisionPoll> AddDecisionPollAsync(DecisionPoll decisionPoll)
        {
            DecisionPoll decisionPollWithAddAuditApplied = await ApplyAddDecisionPollAsync(decisionPoll);
            await ValidateDecisionPollOnAddAsync(decisionPollWithAddAuditApplied);

            return await this.storageBroker.InsertDecisionPollAsync(decisionPollWithAddAuditApplied);
        }

        virtual internal async ValueTask<DecisionPoll> ApplyAddDecisionPollAsync(DecisionPoll decisionPoll)
        {
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            decisionPoll.CreatedBy = auditUser?.EntraUserId ?? string.Empty;
            decisionPoll.CreatedDate = auditDateTimeOffset;
            decisionPoll.UpdatedBy = auditUser?.EntraUserId ?? string.Empty;
            decisionPoll.UpdatedDate = auditDateTimeOffset;

            return decisionPoll;
        }
    }
}
