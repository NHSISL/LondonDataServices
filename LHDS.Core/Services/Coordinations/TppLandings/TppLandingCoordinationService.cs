// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.TppLandings;

namespace LHDS.Core.Services.Coordinations.TppLandings
{
    public partial class TppLandingCoordinationService : ITppLandingCoordinationService
    {
        private readonly ITppLandingOrchestrationService tppOrchestrationService;
        private readonly IIngressOrchestrationService ingressOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public TppLandingCoordinationService(
            ITppLandingOrchestrationService tppOrchestrationService,
            IIngressOrchestrationService ingressOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.tppOrchestrationService = tppOrchestrationService;
            this.ingressOrchestrationService = ingressOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guid> ProcessAsync(string fileName, Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateArgumentsOnProcess(fileName, supplierId);

                Guid ingestionTrackingId = await this.tppOrchestrationService.ProcessAsync(
                    fileName: fileName,
                    supplierId: supplierId);

                await this.ingressOrchestrationService
                    .CheckForBatchCompleteAsync(ingestionTrackingId);

                return ingestionTrackingId;
            });
    }
}
