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
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Decisions;
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
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobContainers blobContainers;
        private readonly DecisionConfiguration decisionConfiguration;

        public DecisionOrchestrationService(
            IDecisionPollService decisionPollService,
            IDecisionService decisionService,
            IDocumentService documentService,
            ICsvHelperBroker csvHelperBroker,
            IHashBroker hashBroker,
            IIdentifierBroker identifierBroker,
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
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.blobContainers = blobContainers;
            this.decisionConfiguration = decisionConfiguration;
        }

        public ValueTask<List<Decision>> GetPatientDecisions() =>
            TryCatch(async () =>
            {
                ValidateBlobContainersIsNotNull();
                ValidateDecisionConfigurationIsNotNull();

                IQueryable<DecisionPoll> decisionPolls =
                    await this.decisionPollService.RetrieveAllDecisionPollsAsync();

                DecisionPoll maybeDecisionPoll = decisionPolls.FirstOrDefault();

                if (maybeDecisionPoll is null)
                {
                    DecisionPoll newDecisionPoll = new DecisionPoll
                    {
                        Id = await this.identifierBroker.GetIdentifierAsync(),
                        LastPoll = DateTimeOffset.MinValue
                    };

                    maybeDecisionPoll = await this.decisionPollService.AddDecisionPollAsync(newDecisionPoll);
                }

                ValidateDecisionPoll(maybeDecisionPoll);
                DecisionPoll lastDecisionPoll = maybeDecisionPoll;
                DateTimeOffset? lastPollDate = lastDecisionPoll?.LastPoll;
                DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

                List<Decision> decisions =
                    await this.decisionService.GetPatientDecisions(lastPollDate ?? DateTimeOffset.MinValue);

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

                string fileName = $"{this.decisionConfiguration.FolderName}/" +
                    $"{currentPollDate:yyyyMMdd}/" +
                    $"{this.decisionConfiguration.FilePrefix}_{currentPollDate:yyyyMMddHHmmss}.csv";

                string container = this.blobContainers.Ingress;
                string tempFile = Path.GetTempFileName();
                ValidateDocumentRequirements(processedData);

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

                lastDecisionPoll!.LastPoll = currentPollDate;
                await this.decisionPollService.ModifyDecisionPollAsync(lastDecisionPoll);

                return decisions;
            });
    }
}
