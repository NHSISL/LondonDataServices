// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService : IIngestionTrackingBroker
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
                ValidateIngestionTrackingOnAdd(ingestionTracking);

                return await this.storageBroker.InsertIngestionTrackingAsync(ingestionTracking);
            });

        public IQueryable<IngestionTracking> RetrieveAllIngestionTracking() =>
            TryCatch(() => this.storageBroker.SelectAllIngestionTracking());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(string ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                return maybeIngestionTracking;
            });

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingOnModify(ingestionTracking);

                IngestionTracking maybeIngestionTracking =
                    await this.storageBroker.SelectIngestionTrackingByIdAsync(ingestionTracking.Id);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTracking.Id);

                ValidateAgainstStorageIngestionTrackingOnModify(
                    inputIngestionTracking: ingestionTracking,
                    storageIngestionTracking: maybeIngestionTracking);

                return await this.storageBroker.UpdateIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(string ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                return await this.storageBroker.DeleteIngestionTrackingAsync(maybeIngestionTracking);
            });
    }
}