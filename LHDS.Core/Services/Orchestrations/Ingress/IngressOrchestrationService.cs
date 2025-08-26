// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.EmisLandings;
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
        private readonly LandingConfiguration landingConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public IngressOrchestrationService(
            IIngestionTrackingProcessingService ingestionTrackingProcessingService,
            ISpecificationObjectProcessingService specificationObjectProcessingService,
            IDocumentProcessingService documentProcessingService,
            LandingConfiguration landingConfiguration,
            BlobContainers blobContainers,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.ingestionTrackingProcessingService = ingestionTrackingProcessingService;
            this.specificationObjectProcessingService = specificationObjectProcessingService;
            this.documentProcessingService = documentProcessingService;
            this.landingConfiguration = landingConfiguration;
            this.blobContainers = blobContainers;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask ProcessDecryptedItemsForBatchCompleteAsync() =>
            TryCatch(async () =>
            {
                List<Exception> exceptions = new List<Exception>();
                Guid ingestionTrackingId;
                var dateTimeCheck = (await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync()).AddMinutes(-15);

                while ((ingestionTrackingId = (await this.ingestionTrackingProcessingService
                    .RetrieveAllIngestionTrackingsAsync())

                    .Where(ingestionTracking =>
                        ingestionTracking.IsBatchComplete == false &&
                            (ingestionTracking.LastBatchCompleteCheck == null
                                || ingestionTracking.LastBatchCompleteCheck <= dateTimeCheck))

                    .GroupBy(ingestionTracking =>
                        new { ingestionTracking.Batch, ingestionTracking.SubscriberAgreementId })

                    .Where(group =>
                        group.All(ingestionTracking =>
                            ingestionTracking.IsDownloaded
                            && ingestionTracking.Decrypted
                            && ingestionTracking.SubscriberAgreementId != null))

                    .Select(group => group.Select(ingestionTracking => ingestionTracking.Id).FirstOrDefault())
                    .FirstOrDefault()) != default)
                {
                    try
                    {
                        Console.WriteLine($"Checking batch completion for IngestionTrackingId: {ingestionTrackingId}");
                        await this.CheckForBatchCompleteAsync(ingestionTrackingId);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }

                if (exceptions.Any())
                {
                    AggregateException aggregateException = new AggregateException(
                        "One or more errors occurred while checking for batch completion.",
                        exceptions);

                    throw new BatchCompleteException(
                        message: "One or more errors occurred while checking for batch completion.",
                        innerException: aggregateException);
                }
            });

        virtual internal ValueTask CheckForBatchCompleteAsync(Guid ingestionTrackingId) =>
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
                .RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference: ingestionTracking.Batch,
                    subscriberAgreementId: ingestionTracking.SubscriberAgreementId.Value,
                    decrypted: true);

            List<string> missingSpecificationObjectIds = specificationObjectIds
                .Except(decryptedIngestiontrackingObjects).ToList();

            if (missingSpecificationObjectIds.Any())
            {
                isBatchComplete = false;

                string message =
                    $"Checking IngestionTrackingId {ingestionTrackingId} for subscriber agreement " +
                    $"'{ingestionTracking.SubscriberAgreementId}' and batch '{ingestionTracking.Batch}' " +
                    $"Batch is not complete. " +
                    $"Missing specification object files: {string.Join(", ", missingSpecificationObjectIds)}";

                Console.WriteLine(message);
                await this.loggingBroker.LogInformationAsync(message);
            }

            string batchReadyFileName = this.landingConfiguration.BatchReadyFile;

            string batchCompleteFileName =
                $"{ingestionTracking.BatchReadyFolderPath}/{batchReadyFileName}"
                .Replace("\\", "/");

            if (isBatchComplete)
            {
                string batchComplete =
                    $"All specification object files present for subscriber agreement " +
                        $"'{ingestionTracking.SubscriberAgreementId}' and batch '{ingestionTracking.Batch}' " +
                            $"as defined in Dataset Specification Id: '{ingestionTracking.DataSetSpecificationId}'.";

                Console.WriteLine(batchComplete);
                await this.loggingBroker.LogInformationAsync(batchComplete);
                Stream data = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(batchComplete));

                try
                {
                    await this.documentProcessingService.RemoveDocumentByFileNameAsync(
                        fileName: batchCompleteFileName,
                        container: this.blobContainers.Ingress);
                }
                catch (Exception)
                { }

                await this.documentProcessingService.AddDocumentAsync(
                    input: data,
                    fileName: batchCompleteFileName,
                    container: this.blobContainers.Ingress);

                await this.ingestionTrackingProcessingService
                    .MarkAsBatchCompleteAsync(ingestionTrackingId, isBatchComplete: true);

                await this.auditBroker.LogInformationAsync(
                    auditType: "BatchComplete",

                    title: $"{batchReadyFileName} generated for subscriber agreement " +
                        $"'{ingestionTracking.SubscriberAgreementId}' and " +
                            $"batch '{ingestionTracking.Batch}'",

                    message: batchComplete,
                    fileName: batchCompleteFileName,
                    correlationId: ingestionTracking.Batch);
            }
            else
            {
                await this.ingestionTrackingProcessingService
                    .MarkAsBatchCompleteAsync(ingestionTrackingId, isBatchComplete: false);
            }
        });

        public ValueTask RollbackIngestionTrackingItemAsync(string encryptedFileName) =>
        TryCatch(async () =>
        {
            ValidateOnRollbackIngestionTrackingItem(encryptedFileName);
            var query = await this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackingsAsync();

            IngestionTracking maybeIngestionTracking =
                query.FirstOrDefault(ingestionTracking => ingestionTracking.EncryptedFileName == encryptedFileName);

            ValidateStorageIngestionTracking(maybeIngestionTracking, encryptedFileName);
            maybeIngestionTracking.IsProcessing = false;

            await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(maybeIngestionTracking);
        });
    }
}
