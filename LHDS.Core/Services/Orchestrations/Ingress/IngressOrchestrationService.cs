// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.SpecificationObjects;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationService : IIngressOrchestrationService
    {
        private readonly IIngestionTrackingProcessingService ingestionTrackingProcessingService;
        private readonly ISpecificationObjectProcessingService specificationObjectProcessingService;
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public IngressOrchestrationService(
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDocumentProcessingService documentProcessingService,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask CheckForBatchCompleteAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
        {
            ValidateOnCheckForBatchComplete(ingestionTrackingId);

            IngestionTracking ingestionTracking =
                await this.ingestionTrackingProcessingService
                    .RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

            ValidateStorageIngestionTracking(ingestionTracking, ingestionTrackingId);

            List<string> specificationObjectIds = await this.specificationObjectProcessingService
                .RetrieveSpecificationObjectsByDataSetSpecificationId(ingestionTracking.DataSetSpecificationId);

            bool isBatchComplete = true;

            if (specificationObjectIds is null || specificationObjectIds.Count == 0)
            {
                throw new NoConfigIngressOrchestrationException(
                    "No specification object files found for dataset specification id: " +
                    $"{ingestionTracking.DataSetSpecificationId}");
            }

            List<string> ingestiontrackingObject = await this.ingestionTrackingProcessingService
                .RetrieveObjectsInBatchByBatchReference(ingestionTracking.Batch);

            foreach (string specificationObjectId in specificationObjectIds)
            {
                if (!ingestiontrackingObject.Contains(specificationObjectId))
                {
                    isBatchComplete = false;
                    break;
                }
            }

            if (isBatchComplete)
            {
                string batchCompleteFileName =
                    $"{ingestionTracking.BatchReadyFolderPath}/BatchReady.txt"
                    .Replace("\\", "/");

                string batchComplete =
                    $"All specification object files present for dataset specification id: " +
                    $"{ingestionTracking.DataSetSpecificationId}";

                Stream data = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(batchComplete));

                await this.documentProcessingService.AddDocumentAsync(
                    input: data,
                    fileName: batchCompleteFileName,
                    container: ingestionTracking.Container);
            }
        });
    }
}
