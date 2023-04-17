// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.Mesh;
using LHDS.Core.Services.Processings.OptOuts;
using LHDS.Core.Tests.Unit.Services.Processings.CsvMappers;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService : IOptOutOrchestrationService
    {
        private readonly IOptOutProcessingService optOutProcessingService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IMeshProcessingService meshProcessingService;
        private readonly ICsvMapperProcessingService csvMapperProcessingService;

        public OptOutOrchestrationService(
            IOptOutProcessingService optOutProcessingService,
            IDocumentProcessingService documentProcessingService,
            IMeshProcessingService meshProcessingService,
            ICsvMapperProcessingService csvMapperProcessingService)
        {
            this.optOutProcessingService = optOutProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.meshProcessingService = meshProcessingService;
            this.csvMapperProcessingService = csvMapperProcessingService;
        }

        public ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile) =>
            throw new NotImplementedException();

        public ValueTask PushExpiredOptOutsToMeshForRenewalAsync() =>
            throw new NotImplementedException();

        public ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync() =>
            throw new NotImplementedException();
    }
}
