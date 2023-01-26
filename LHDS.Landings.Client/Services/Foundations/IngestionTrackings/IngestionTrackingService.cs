// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages;
using LHDS.Landings.Client.Models.IngestionTracking;

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
            throw new System.NotImplementedException();

        public IQueryable<IngestionTracking> RetrieveAllIngestionTracking() =>
            throw new System.NotImplementedException();

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            throw new System.NotImplementedException();

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            throw new System.NotImplementedException();

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            throw new System.NotImplementedException();
    }
}
