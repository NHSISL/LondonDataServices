// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Decisions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Services.Decisions
{
    public partial class DecisionService : IDecisionService
    {
        private readonly IDecisionBroker decisionBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public DecisionService(
            IDecisionBroker decisionBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.decisionBroker = decisionBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Decision>> GetPatientDecisions(DateTimeOffset? lastPollDate) =>
            TryCatch(async () =>
            {
                List<Decision> maybeDecisions =
                    await this.decisionBroker.GetPatientDecisions(lastPollDate);

                ValidateDecisions(maybeDecisions);

                return maybeDecisions;
            });
    }
}
