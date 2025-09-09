// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Decisions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Decisions;

namespace LHDS.Core.Services.Decisions
{
    public partial class DecisionService : IDecisionService
    {
        private readonly IDecisionBroker decisionBroker;
        private readonly ILoggingBroker loggingBroker;

        public DecisionService(
            IDecisionBroker decisionBroker,
            ILoggingBroker loggingBroker)
        {
            this.decisionBroker = decisionBroker;
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
