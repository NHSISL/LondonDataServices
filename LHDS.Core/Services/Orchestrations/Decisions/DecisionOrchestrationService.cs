// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
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
            DecisionConfiguration decisionConfiguration)
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

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(DecisionCsv.DecisionId), 0 },
                    { nameof(DecisionCsv.NhsNumber), 1 },
                    { nameof(DecisionCsv.PatientInstructionCategory), 2 },
                    { nameof(DecisionCsv.PatientInstructionState), 3 },
                    { nameof(DecisionCsv.InstructionDate), 4 }
                };

                string fileName =
                    $"{this.decisionConfiguration.FolderName}/" +
                    $"{currentPollDate:yyyyMMddHHmmss}/" +
                    $"{this.decisionConfiguration.FilePrefix}_{currentPollDate:yyyyMMddHHmmss}.csv";

                string container = this.blobContainers.Ingress;
                ValidateDocumentRequirements(fileName);
                Pipe pipe = new Pipe();

                using HashingCountingBroker hashingStream = new HashingCountingBroker(
                    pipe.Reader.AsStream(),
                    HashAlgorithmName.SHA256);

                async Task WriteCsvToPipeAsync()
                {
                    try
                    {
                        await using Stream writerStream = pipe.Writer.AsStream();

                        await this.csvHelperBroker.MapObjectToCsvAsync<DecisionCsv>(
                            @object: ProjectDecisionsAsync(decisions),
                            outputStream: writerStream,
                            addHeaderRecord: true,
                            fieldMappings: fieldMappings,
                            shouldAddTrailingComma: false);
                    }
                    catch (Exception ex)
                    {
                        await pipe.Writer.CompleteAsync(ex);

                        return;
                    }

                    await pipe.Writer.CompleteAsync();
                }

                await Task.WhenAll(
                    WriteCsvToPipeAsync(),
                    this.documentService.AddDocumentAsync(
                        input: hashingStream.AsStream(),
                        fileName: fileName,
                        container: container).AsTask());

                await this.loggingBroker.LogInformationAsync(
                    $"CSV uploaded: {hashingStream.BytesRead} bytes, " +
                    $"SHA256: {hashingStream.GetFinalHashHex()}, " +
                    $"File: {fileName}");

                await this.decisionService.RecordAdoption(decisions);

                return decisions;
            });

        private async IAsyncEnumerable<DecisionCsv> ProjectDecisionsAsync(
            List<Decision> decisions,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (Decision decision in decisions)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string nhsNumber = this.decisionConfiguration.HashNhsNumber
                    ? await this.hashBroker.GenerateSha256HashAsync(
                        decision.PatientNhsNumber,
                        this.decisionConfiguration.HashPepper)
                    : decision.PatientNhsNumber;

                yield return new DecisionCsv
                {
                    DecisionId = decision.Id,
                    NhsNumber = nhsNumber,
                    PatientInstructionCategory = decision.DecisionTypeName,
                    PatientInstructionState = decision.DecisionChoice,
                    InstructionDate = decision.CreatedDate
                };
            }
        }
    }
}
