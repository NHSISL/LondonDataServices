// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
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
                ValidateIngestionTrackingOnAdd(ingestionTracking);

                return await this.storageBroker.InsertIngestionTrackingAsync(ingestionTracking);
            });

        public IQueryable<IngestionTracking> RetrieveAllIngestionTracking() =>
            TryCatch(() => this.storageBroker.ReadAllIngestionTracking());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(string ingestionTrackingId) =>
        TryCatch(async () =>
        {
            ValidateIngestionTrackingId(ingestionTrackingId);

            IngestionTracking maybeIngestionTracking = await this.storageBroker
                .ReadIngestionTrackingByIdAsync(ingestionTrackingId);

            ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

            return maybeIngestionTracking;
        });

        public async ValueTask<IngestionTracking> RetrieveIngestionTrackingByFileNameAsync(string fileName) =>
             await this.storageBroker.ReadIngestionTrackingByFileNameAsync(fileName);

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            throw new System.NotImplementedException();

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(string ingestionTrackingId) =>
            throw new System.NotImplementedException();
    }
}
