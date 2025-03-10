// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
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
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public IngressOrchestrationService(
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDocumentProcessingService documentProcessingService,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.blobContainers = blobContainers;
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
                .RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(ingestionTracking.DataSetSpecificationId);

            bool isBatchComplete = true;

            if (specificationObjectIds is null || specificationObjectIds.Count == 0)
            {
                throw new NoConfigIngressOrchestrationException(
                    "No specification object files found for dataset specification id: " +
                    $"{ingestionTracking.DataSetSpecificationId}");
            }

            List<string> decryptedIngestiontrackingObjects = await this.ingestionTrackingProcessingService
                .RetrieveObjectsInBatchByBatchReference(bacthReference: ingestionTracking.Batch, decrypted: true);

            List<string> missingSpecificationObjectIds = specificationObjectIds
                .Except(decryptedIngestiontrackingObjects).ToList();

            if (missingSpecificationObjectIds.Any())
            {
                isBatchComplete = false;
            }

            string batchCompleteFileName =
                $"{ingestionTracking.BatchReadyFolderPath}/BatchReady.txt"
                .Replace("\\", "/");

            string batchIncompleteFileName =
                $"{ingestionTracking.BatchReadyFolderPath}/BatchNotReady.txt"
                .Replace("\\", "/");

            if (isBatchComplete)
            {
                string batchComplete =
                    $"All specification object files present for batch '{ingestionTracking.Batch}' " +
                    $"as defined in Dataset Specification Id: '{ingestionTracking.DataSetSpecificationId}'." +
                    Environment.NewLine +
                    $"Generate batch complete file: '{batchCompleteFileName}'";

                Stream data = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(batchComplete));

                await this.documentProcessingService.AddDocumentAsync(
                    input: data,
                    fileName: batchCompleteFileName,
                    container: this.blobContainers.Ingress);

                await this.auditBroker.LogInformationAsync(
                    auditType: "BatchComplete",
                    title: "BatchReady.txt generated",
                    message: batchComplete,
                    fileName: batchCompleteFileName,
                    correlationId: ingestionTracking.Batch);

                try
                {
                    await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                        batchIncompleteFileName,
                        this.blobContainers.Ingress);
                }
                catch (Exception)
                { }
            }
            else
            {
                string batchIncomplete =
                    $"Unable to generate '{batchCompleteFileName}' for batch: {ingestionTracking.Batch}.  " +
                    Environment.NewLine +
                    $"We are missing {missingSpecificationObjectIds.Count}/{specificationObjectIds.Count} files.  " +
                    Environment.NewLine +
                    $"Missing specification object Id's: {string.Join(", ", missingSpecificationObjectIds)} " +
                    Environment.NewLine +
                    $"as defined by Dataset Specification Id: {ingestionTracking.DataSetSpecificationId}";

                Stream data = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(batchIncomplete));

                await this.documentProcessingService.AddDocumentAsync(
                    input: data,
                    fileName: batchIncompleteFileName,
                    container: this.blobContainers.Ingress);

                await this.auditBroker.LogInformationAsync(
                    auditType: "BatchComplete",
                    title: "Unable to generate BatchReady.txt",
                    message: batchIncomplete,
                    fileName: batchCompleteFileName,
                    correlationId: ingestionTracking.Batch);

                try
                {
                    await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                        batchCompleteFileName,
                        this.blobContainers.Ingress);
                }
                catch (Exception)
                { }
            }
        });
    }
}
