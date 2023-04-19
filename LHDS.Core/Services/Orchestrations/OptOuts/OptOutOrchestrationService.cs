// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
                ValidateOptOutFileIsNotNull(optOutFile);
                ValidateRequestIdIsNotNull(requestId);

                var inputString = Encoding.ASCII.GetString(optOutFile);

                List<OptOut> mappedOptOuts =
                await this.csvMapperProcessingService.MapCsvToObjectAsync<OptOut>(inputString, false);

                List<OptOut> processedOptOuts = new List<OptOut>();

                foreach (var optOut in mappedOptOuts)
                {
                    processedOptOuts.Add(await this.optOutProcessingService.RetrieveOrAddOptOutAsync(optOut));
                }

                var processedString = await this.csvMapperProcessingService
                    .MapObjectToCsvAsync(processedOptOuts, false);

                var processedBytes = Encoding.ASCII.GetBytes(processedString);

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
                //TODO: Validate you can get configuration settings
                //ValidateConfigurationSettingsNotNull(optOutConfiguration.ExpiredAfterDays);

                List<OptOut> mappedOptOuts = await
                this.optOutProcessingService.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays);

                var processedOutputString = await this.csvMapperProcessingService
                       .MapObjectToCsvAsync(mappedOptOuts, false);

                MeshMessage message = new MeshMessage
                {
                    StringContent = processedOutputString
                };

                await this.meshProcessingService.SendMessageAsync(message);
            });

        public ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
