// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService : IIngestionTrackingService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                await ValidateIngestionTrackingOnAddAsync(ingestionTracking);

                return await this.storageBroker.InsertIngestionTrackingAsync(ingestionTracking);
            });

        public IQueryable<IngestionTracking> RetrieveAllIngestionTrackings() =>
            TryCatch(() => this.storageBroker.SelectAllIngestionTrackings());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                return maybeIngestionTracking;
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByFileNameAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(fileName);

                IngestionTracking? maybeIngestionTracking =
                    this.storageBroker.SelectAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByEncryptedFileNameAsync(string encryptedFileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(encryptedFileName);

                IngestionTracking? maybeIngestionTracking =
                    this.storageBroker.SelectAllIngestionTrackings()
                        .FirstOrDefault(ingestionTracking => ingestionTracking.EncryptedFileName == encryptedFileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                await ValidateIngestionTrackingOnModifyAsync(ingestionTracking);

                IngestionTracking maybeIngestionTracking =
                    await this.storageBroker.SelectIngestionTrackingByIdAsync(ingestionTracking.Id);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId: ingestionTracking.Id);

                ValidateAgainstStorageIngestionTrackingOnModify(
                    inputIngestionTracking: ingestionTracking,
                    storageIngestionTracking: maybeIngestionTracking);

                return await this.storageBroker.UpdateIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateIngestionTrackingId(ingestionTrackingId: ingestionTrackingId);

                    IngestionTracking maybeIngestionTracking = await this.storageBroker
                        .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                    ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                    return await this.storageBroker.DeleteIngestionTrackingAsync(maybeIngestionTracking);
                });
            });
    }
}