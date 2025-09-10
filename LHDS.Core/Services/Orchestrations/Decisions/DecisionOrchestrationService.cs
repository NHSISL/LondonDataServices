// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Services.Foundations.DecisionPolls;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public class DecisionOrchestrationService : IDecisionOrchestrationService
    {
        private readonly IDecisionPollService decisionPollService;
        private readonly IDecisionService decisionService;
        private readonly IDocumentService documentService;
        private readonly BlobContainers blobContainers;

        public DecisionOrchestrationService(
            IDecisionPollService decisionPollService,
            IDecisionService decisionService,
            IDocumentService documentService,
            BlobContainers blobContainers
            )
        {
            this.decisionPollService = decisionPollService;
            this.decisionService = decisionService;
            this.documentService = documentService;
            this.blobContainers = blobContainers;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            throw new NotImplementedException();
        }
    }
}
