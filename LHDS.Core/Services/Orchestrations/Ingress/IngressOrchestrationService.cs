// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.IngestionTrackings;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public class IngressOrchestrationService : IIngressOrchestrationService
    {
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public IngressOrchestrationService(
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            IDataSetSpecificationProcessingService dataSetSpecificationProcessingService,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.dataSetSpecificationProcessingService = dataSetSpecificationProcessingService;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask CheckForBatchCompleteAsync(Guid ingestionTrackingId) =>
            ValueTask.CompletedTask;
    }
}
