// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Services.Orchestrations.Decisions;

namespace LHDS.Core.Clients
{
    public class IDecideClient : IIDecideClient
    {
        private readonly IDecisionOrchestrationService decisionOrchestrationService;

        public IDecideClient(IDecisionOrchestrationService decisionOrchestrationService)
        {
            this.decisionOrchestrationService = decisionOrchestrationService;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            return await this.decisionOrchestrationService.GetPatientDecisions();
        }
    }
}
