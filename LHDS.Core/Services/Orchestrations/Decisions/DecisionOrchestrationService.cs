// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationService : IDecisionOrchestrationService
    {
        private readonly IDecisionService decisionService;
        private readonly IDocumentService documentService;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IHashBroker hashBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobContainers blobContainers;
        private readonly DecisionConfiguration decisionConfiguration;

        public DecisionOrchestrationService(
            IDecisionService decisionService,
            IDocumentService documentService,
            ICsvHelperBroker csvHelperBroker,
            IHashBroker hashBroker,
            ILoggingBroker loggingBroker,
            BlobContainers blobContainers,
            DecisionConfiguration decisionConfiguration
            )
        {
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
                ValidateDecisionConfigurationIsNotNull();

                List<Decision> decisions =
                    await this.decisionService.GetPatientDecisions();

                if (decisions.Count == 0)
                {
                    return decisions;
                }

                DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

                List<Task<DecisionCsv>> decisionCsvTasks = decisions
                    .Select(async decision => new DecisionCsv
                    {
                        DecisionId = decision.Id,

                        NhsNumber = this.decisionConfiguration.HashNhsNumber
                            ? await this.hashBroker.GenerateSha256HashAsync(
                                decision.PatientNhsNumber,
                                this.decisionConfiguration.HashPepper)
                            : decision.PatientNhsNumber,

                        PatientInstructionCategory = decision.DecisionTypeName,
                        PatientInstructionState = decision.DecisionChoice,
                        InstructionDate = decision.CreatedDate
                    })
                    .ToList();

                DecisionCsv[] decisionCsvs = await Task.WhenAll(decisionCsvTasks);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(DecisionCsv.DecisionId), 0 },
                    { nameof(DecisionCsv.NhsNumber), 1 },
                    { nameof(DecisionCsv.PatientInstructionCategory), 2 },
                    { nameof(DecisionCsv.PatientInstructionState), 3 },
                    { nameof(DecisionCsv.InstructionDate), 4 }
                };

                string fileName = $"{this.decisionConfiguration.FolderName}/" +
                    $"{currentPollDate:yyyyMMddHHmmss}/" +
                    $"{this.decisionConfiguration.FilePrefix}_{currentPollDate:yyyyMMddHHmmss}.csv";

                string container = this.blobContainers.Ingress;
                string tempFile = Path.GetTempFileName();

                //TODO: Review this implementation to avoid memory issues with large files.
                //Consider streaming directly to blob storage if possible.
                try
                {
                    using (Stream fileStream = new FileStream(
                               tempFile,
                               FileMode.Create,
                               FileAccess.Write,
                               FileShare.None))
                    {
                        await this.csvHelperBroker
                            .MapObjectToCsvAsync(
                                @object: decisionCsvs.ToList(),
                                outputStream: fileStream,
                                addHeaderRecord: true,
                                fieldMappings: fieldMappings,
                                shouldAddTrailingComma: false);
                    }

                    ValidateDocumentRequirements(fileName);

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

                return decisions;
            });
    }
}
