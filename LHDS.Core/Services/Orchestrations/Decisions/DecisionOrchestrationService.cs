// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.DecisionConfigurations;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Services.Foundations.DecisionPolls;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationService : IDecisionOrchestrationService
    {
        private readonly IDecisionPollService decisionPollService;
        private readonly IDecisionService decisionService;
        private readonly IDocumentService documentService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IHashBroker hashBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobContainers blobContainers;
        private readonly DecisionConfiguration decisionConfiguration;

        public DecisionOrchestrationService(
            IDecisionPollService decisionPollService,
            IDecisionService decisionService,
            IDocumentService documentService,
            ICsvHelperBroker csvHelperBroker,
            IHashBroker hashBroker,
            ILoggingBroker loggingBroker,
            BlobContainers blobContainers,
            DecisionConfiguration decisionConfiguration
            )
        {
            this.decisionPollService = decisionPollService;
            this.decisionService = decisionService;
            this.documentService = documentService;
            this.csvHelperBroker = csvHelperBroker;
            this.hashBroker = hashBroker;
            this.loggingBroker = loggingBroker;
            this.blobContainers = blobContainers;
            this.decisionConfiguration = decisionConfiguration;
        }

        public ValueTask<List<Decision>> GetPatientDecisions() =>
            TryCatch(async () =>
            {
                ValidateBlobContainersIsNotNull();

                IQueryable<DecisionPoll> decisionPolls =
                    await this.decisionPollService.RetrieveAllDecisionPollsAsync();

                DateTimeOffset? lastPollDate = decisionPolls
                    .OrderByDescending(decisionPoll => decisionPoll.LastPoll)
                    .Select(decisionPoll => decisionPoll.LastPoll)
                    .FirstOrDefault();

                ValidateDecisionPolls(decisionPolls);

                DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

                List<Decision> decisions =
                    await this.decisionService.GetPatientDecisions(lastPollDate);

                List<Task<DecisionCsv>> decisionCsvTasks = decisions
                    .Select(async decision => new DecisionCsv
                    {
                        DecisionId = decision.Id,
                        NhsHash = await this.hashBroker.GenerateSha256HashAsync(
                            decision.Patient.NhsNumber,
                            this.decisionConfiguration.HashPepper),
                        PatientInstructionCategory = decision.DecisionType.Name,
                        PatientInstructionState = decision.DecisionChoice,
                        InstructionDate = decision.CreatedDate
                    })
                    .ToList();

                DecisionCsv[] decisionCsvs = await Task.WhenAll(decisionCsvTasks);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(DecisionCsv.DecisionId), 0 },
                    { nameof(DecisionCsv.NhsHash), 1 },
                    { nameof(DecisionCsv.PatientInstructionCategory), 2 },
                    { nameof(DecisionCsv.PatientInstructionState), 3 },
                    { nameof(DecisionCsv.InstructionDate), 4 }
                };

                string processedData = await this.csvHelperBroker
                    .MapObjectToCsvAsync(
                        @object: decisionCsvs.ToList(),
                        addHeaderRecord: true,
                        fieldMappings: fieldMappings,
                        shouldAddTrailingComma: false);

                string fileName = $"IDecide_{currentPollDate:HHmm_ddMMyyyy}";
                string container = this.blobContainers.Decisions;
                string tempFile = Path.GetTempFileName();

                try
                {
                    using (Stream fileStream = new FileStream(
                               tempFile,
                               FileMode.Create,
                               FileAccess.Write,
                               FileShare.None))
                    {
                        fileStream.Write(Encoding.UTF8.GetBytes(processedData));
                    }

                    using (Stream tempDocument = new FileStream(
                               tempFile,
                               FileMode.Open,
                               FileAccess.Read,
                               FileShare.Read))
                    {
                        await this.documentService.AddDocumentAsync(
                            input: tempDocument,
                            fileName: fileName,
                            container: container);
                    }
                }
                finally
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }

                await this.decisionService.RecordAdoption(decisions);

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

                return decisions;
            });
    }
}
