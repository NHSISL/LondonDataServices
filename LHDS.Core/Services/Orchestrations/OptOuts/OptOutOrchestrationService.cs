// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.OptOuts;
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

        public OptOutOrchestrationService(
            IOptOutProcessingService optOutProcessingService,
            IDocumentProcessingService documentProcessingService,
            IMeshProcessingService meshProcessingService,
            ICsvMapperProcessingService csvMapperProcessingService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.optOutProcessingService = optOutProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.meshProcessingService = meshProcessingService;
            this.csvMapperProcessingService = csvMapperProcessingService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string requestId) =>
            TryCatch(async () =>
            {
                ValidateOptOutFileIsNotNull(optOutFile);
                ValidateRequestIdIsNotNull(requestId);

                List<OptOut> mappedOptOuts =
                await this.csvMapperProcessingService.MapCsvDataToObjectAsync<OptOut>(optOutFile);

                List<OptOut> processedOptOuts = new List<OptOut>();

                foreach (var optOut in mappedOptOuts)
                {
                    processedOptOuts.Add(await this.optOutProcessingService.RetrieveOrAddOptOutAsync(optOut));
                }

                var processedBytes = await this.csvMapperProcessingService.MapObjectToCsvDataAsync(processedOptOuts);

                Document document = new Document
                {
                    FileName = $"receive/{requestId}_Response_{dateTimeBroker.GetCurrentDateTimeOffset()}.csv",
                    DocumentData = processedBytes
                };

                await this.documentProcessingService.AddDocumentAsync(document);
            });

        public ValueTask PushExpiredOptOutsToMeshForRenewalAsync() =>
            throw new NotImplementedException();

        public ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync() =>
            throw new NotImplementedException();
    }
}
