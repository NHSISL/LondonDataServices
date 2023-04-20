// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Services.Processings.CsvMappers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService : IOptOutOrchestrationService
    {
        private readonly IOptOutProcessingService optOutProcessingService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IMeshProcessingService meshProcessingService;
        private readonly ICsvMapperProcessingService csvMapperProcessingService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly OptOutConfiguration optOutConfiguration;

        public OptOutOrchestrationService(
            IOptOutProcessingService optOutProcessingService,
            IDocumentProcessingService documentProcessingService,
            IMeshProcessingService meshProcessingService,
            ICsvMapperProcessingService csvMapperProcessingService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            OptOutConfiguration optOutConfiguration)
        {
            this.optOutProcessingService = optOutProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.meshProcessingService = meshProcessingService;
            this.csvMapperProcessingService = csvMapperProcessingService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.optOutConfiguration = optOutConfiguration;
        }

        public ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string requestId) =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                ValidateOptOutFileIsNotNull(optOutFile);
                ValidateRequestIdIsNotNull(requestId);

                bool withHeader =
                    optOutConfiguration.OptOutFileHasHeader;

                bool shouldAddTrailingComma =
                    optOutConfiguration.OptOutFileRequireTrailingComma;

                var inputString = Encoding.ASCII.GetString(optOutFile);

                List<OptOut> mappedOptOuts =
                await this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(inputString, false);

                List<OptOut> processedOptOuts = new List<OptOut>();

                foreach (var optOut in mappedOptOuts)
                {
                    processedOptOuts.Add(await this.optOutProcessingService.RetrieveOrAddOptOutAsync(optOut));
                }

                var processedData = await this.csvMapperProcessingService
                    .MapObjectToCsvAsync(processedOptOuts, withHeader, shouldAddTrailingComma);

                var processedBytes = Encoding.ASCII.GetBytes(processedData);

                Document document = new Document
                {
                    FileName = $"{optOutConfiguration.OutputFolder}/{requestId}_Response_{dateTimeBroker
                        .GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss")}.csv",
                    DocumentData = processedBytes
                };

                await this.documentProcessingService.AddDocumentAsync(document);
            });

        public ValueTask PushExpiredOptOutsToMeshForRenewalAsync() =>
            TryCatch(async () =>
            {
                ValidateConfigurationSettings();
                bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;
                bool shouldAddTrailingComma = this.optOutConfiguration.OptOutFileRequireTrailingComma;

                List<OptOut> mappedOptOuts = await
                    this.optOutProcessingService
                        .RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays);

                var processedOutputString = await this.csvMapperProcessingService
                       .MapObjectToCsvAsync(mappedOptOuts, withHeader, shouldAddTrailingComma);

                MeshMessage message = new MeshMessage
                {
                    StringContent = processedOutputString
                };

                await this.meshProcessingService.SendMessageAsync(message);
            });

        public async ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            ValidateConfigurationSettings();
            bool withHeader = this.optOutConfiguration.OptOutFileHasHeader;

            List<string> outputMessageIds = await
                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

            foreach (string messageId in outputMessageIds)
            {
                MeshMessage message = await meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(messageId);
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOutIdentifier> outputOptOutIdentifierConsentedList =
                    await csvMapperProcessingService.MapCsvToObjectAsync<OptOutIdentifier>(
                        message.StringContent,
                        withHeader);

                List<OptOut> originalBatch = await this.optOutProcessingService
                    .RetrieveAllOptOutsByBatchReferenceAsync(batchReference);

                List<string> consentedIdentifiers = outputOptOutIdentifierConsentedList
                    .Select(optOut => optOut.NhsNumber).ToList();

                List<OptOut> consentedOptouts = originalBatch
                    .Where(optOut => consentedIdentifiers.Contains(optOut.NhsNumber)).ToList();

                List<OptOut> nonConsentedOptouts = originalBatch.Except(consentedOptouts).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (var item in consentedOptouts)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                        item.UpdatedDate = dateTime;
                        item.CacheTime = dateTime;
                        item.OptOutStatus = "Opt-In";

                        await this.optOutProcessingService.ModifyOptOutAsync(item);
                        delta.Add(item);
                    }
                }

                foreach (var item in nonConsentedOptouts)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        var dateTime = this.dateTimeBroker.GetCurrentDateTimeOffset();

                        item.UpdatedDate = dateTime;
                        item.CacheTime = dateTime;
                        item.OptOutStatus = "Opt-Out";

                        await this.optOutProcessingService.ModifyOptOutAsync(item);
                        delta.Add(item);
                    }
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                    .Select(identifier => new OptOutIdentifier { NhsNumber = identifier.NhsNumber }).ToList();

                string csvDifferences = await this.csvMapperProcessingService
                    .MapObjectToCsvAsync(
                        @object: differentIdentifiers,
                        addHeaderRecord: this.optOutConfiguration.OptOutFileHasHeader,
                        shouldAddTrailingComma: this.optOutConfiguration.OptOutFileRequireTrailingComma);

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
                        $"{this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss")}.csv",
                };

                await this.documentProcessingService.AddDocumentAsync(document);
            }

        }
    }
}
