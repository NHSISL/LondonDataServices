// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Services.Orchestrations.Decisions;

namespace LHDS.Core.Services.Coordinations.Decisions
{
    public partial class DecisionCoordinationService : IDecisionCoordinationService
    {
        private readonly IDecisionOrchestrationService decisionOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public DecisionCoordinationService(
            IDecisionOrchestrationService decisionOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.decisionOrchestrationService = decisionOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            return await this.decisionOrchestrationService.GetPatientDecisions();
        }
    }
}
