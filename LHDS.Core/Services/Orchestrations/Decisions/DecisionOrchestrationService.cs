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
using LHDS.Core.Brokers.Loggings;
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
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobContainers blobContainers;

        public DecisionOrchestrationService(
            IDecisionPollService decisionPollService,
            IDecisionService decisionService,
            IDocumentService documentService,
            ILoggingBroker loggingBroker,
            BlobContainers blobContainers
            )
        {
            this.decisionPollService = decisionPollService;
            this.decisionService = decisionService;
            this.documentService = documentService;
            this.loggingBroker = loggingBroker;
            this.blobContainers = blobContainers;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            IQueryable<DecisionPoll> decisionPolls =
                await this.decisionPollService.RetrieveAllDecisionPollsAsync();

            DateTimeOffset? lastPollDate = decisionPolls
                .OrderByDescending(decisionPoll => decisionPoll.LastPoll)
                .Select(decisionPoll => decisionPoll.LastPoll)
                .FirstOrDefault();

            DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

            List<Decision> decisions =
                await this.decisionService.GetPatientDecisions(lastPollDate);

            string serializedDecisions = JsonSerializer.Serialize(decisions);
            string fileName = $"IDecide_{currentPollDate:HHmm_ddMMyyyy}";
            string container = this.blobContainers.Decisions;
            using var documentStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedDecisions));

            await this.documentService.
                AddDocumentAsync(documentStream, fileName, container);

            var newDecisionPoll = new DecisionPoll
            {
                Id = Guid.NewGuid(),
                LastPoll = currentPollDate,
                CreatedBy = "system",
                CreatedDate = currentPollDate,
                UpdatedBy = "system",
                UpdatedDate = currentPollDate
            };

            await this.decisionPollService.AddDecisionPollAsync(newDecisionPoll);
            await this.decisionService.RecordAdoption(decisions);

            return decisions;
        }
    }
}
