// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingService : IIngestionTrackingProcessingService
    {
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingProcessingService(
            IIngestionTrackingService ingestionTrackingService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.ingestionTrackingService = ingestionTrackingService;
            this.dateTimeBroker = dateTimeBroker;
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
            string batchReference,
            Guid subscriberAgreementId,
            bool? decrypted = null) =>
            TryCatch(async () =>
            {
                ValidateOnRetrieveObjectsInBatchByBatchReference(batchReference, subscriberAgreementId);

                List<string> objectNames = new List<string>();

                IQueryable<IngestionTracking> allingestionTrackings =
                    await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

                allingestionTrackings = allingestionTrackings
                    .Where(ingestionTracking => ingestionTracking.Batch == batchReference
                        && ingestionTracking.SubscriberAgreementId == subscriberAgreementId);

                if (decrypted.HasValue)
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

        public ValueTask<List<string>> RetrieveDecryptedObjectsInBatchByBatchReference(
            string batchReference, Guid supplierId) =>
        TryCatch(async () =>
        {
            ValidateOnRetrieveObjectsInBatchByBatchReference(batchReference, supplierId);

            List<string> objectNames = new List<string>();

            IQueryable<IngestionTracking> allingestionTrackings =
                await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

            List<string?> result = allingestionTrackings

                .Where(ingestionTracking => ingestionTracking.Batch == batchReference
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
                .Where(ingestionTracking =>
                    ingestionTracking.Batch == ingestionTrackingItem.Batch &&
                    ingestionTracking.SubscriberAgreementId == ingestionTrackingItem.SubscriberAgreementId)
                .ToList();

            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            foreach (IngestionTracking batchIngestionTracking in batchIngestionTrackings)
            {
                batchIngestionTracking.IsBatchComplete = isBatchComplete;
                batchIngestionTracking.LastBatchCompleteCheck = currentDateTime;
            }

            await this.ingestionTrackingService.BulkModifyIngestionTrackingAsync(batchIngestionTrackings);
        });
    }
}
