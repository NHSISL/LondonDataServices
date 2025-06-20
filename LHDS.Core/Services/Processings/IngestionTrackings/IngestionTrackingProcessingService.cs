// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingService : IIngestionTrackingProcessingService
    {
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingProcessingService(
            IIngestionTrackingService ingestionTrackingService,
            ILoggingBroker loggingBroker)
        {
            this.ingestionTrackingService = ingestionTrackingService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                ValidateIngestionTracking(ingestionTracking);

                return await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IQueryable<IngestionTracking>> RetrieveAllIngestionTrackingsAsync() =>
            TryCatch(async () => await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                return await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);
            });

        public ValueTask<IngestionTracking> RetrieveOrAddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                ValidateIngestionTracking(ingestionTracking);
                ValidateIngestionTrackingId(ingestionTracking.Id);

                return await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id) ??
                    await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IngestionTracking> ModifyOrAddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                ValidateIngestionTracking(ingestionTracking);
                ValidateIngestionTrackingId(ingestionTracking.Id);

                var maybeIngestionTracking = await this.ingestionTrackingService
                    .RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

                if (maybeIngestionTracking != null)
                {
                    return await this.ingestionTrackingService.ModifyIngestionTrackingAsync(ingestionTracking);
                }
                else
                {
                    return await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
                }
            });

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                ValidateIngestionTracking(ingestionTracking);

                return await this.ingestionTrackingService.ModifyIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                return await this.ingestionTrackingService.RemoveIngestionTrackingByIdAsync(ingestionTrackingId);
            });

        public ValueTask<List<string>> RetrieveObjectsInBatchByBatchReferenceAsync(
            string bacthReference,
            bool? decrypted = null,
            Guid? subscriberAgreementId = null) =>
            TryCatch(async () =>
            {
                ValidateOnRetrieveObjectsInBatchByBatchReference(bacthReference);

                List<string> objectNames = new List<string>();

                IQueryable<IngestionTracking> allingestionTrackings =
                    await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

                allingestionTrackings = allingestionTrackings
                    .Where(ingestionTracking =>
                        ingestionTracking.Batch == bacthReference
                        && ingestionTracking.SubscriberAgreementId == subscriberAgreementId);

                if (decrypted != null)
                {
                    allingestionTrackings = allingestionTrackings
                        .Where(ingestionTracking => ingestionTracking.Decrypted == decrypted.Value);
                }

                List<string?> result = allingestionTrackings
                    .Select(ingestionTracking => ingestionTracking.ObjectName)
                    .ToList();

                result.ForEach(objectName =>
                {
                    if (string.IsNullOrWhiteSpace(objectName) is false)
                    {
                        objectNames.Add(objectName);
                    }
                });

                return objectNames;
            });

        public ValueTask<List<string>> RetrieveDecryptedObjectsInBatchByBatchReference(string bacthReference) =>
        TryCatch(async () =>
        {
            ValidateOnRetrieveObjectsInBatchByBatchReference(bacthReference);

            List<string> objectNames = new List<string>();

            IQueryable<IngestionTracking> allingestionTrackings =
                await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

            List<string?> result = allingestionTrackings

                .Where(ingestionTracking => ingestionTracking.Batch == bacthReference
                    && ingestionTracking.Decrypted == true)

                .Select(ingestionTracking => ingestionTracking.ObjectName)
                .ToList();

            result.ForEach(objectName =>
            {
                if (string.IsNullOrWhiteSpace(objectName) is false)
                {
                    objectNames.Add(objectName);
                }
            });

            return objectNames;
        });

        public ValueTask MarkAsBatchCompleteAsync(Guid ingestionTrackingId, bool isBatchComplete) =>
        TryCatch(async () =>
        {
            ValidateIngestionTrackingId(ingestionTrackingId);

            IngestionTracking ingestionTrackingItem =
                await this.ingestionTrackingService.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId);

            IQueryable<IngestionTracking> allIngestionTrackings =
                await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

            List<IngestionTracking> batchIngestionTrackings = allIngestionTrackings
                .Where(ingestionTracking => ingestionTracking.Batch == ingestionTrackingItem.Batch).ToList();

            foreach (IngestionTracking batchIngestionTracking in batchIngestionTrackings)
            {
                batchIngestionTracking.IsBatchComplete = isBatchComplete;
                await this.ingestionTrackingService.ModifyIngestionTrackingAsync(batchIngestionTracking);
            }
        });
    }
}
