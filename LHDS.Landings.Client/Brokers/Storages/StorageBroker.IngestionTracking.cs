// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTracking> IngestionTrackings { get; set; }

        public async ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await InsertAsync(ingestionTracking);

        public IQueryable<IngestionTracking> ReadAllIngestionTracking() => ReadAll<IngestionTracking>();

        public async ValueTask<IngestionTracking> ReadIngestionTrackingByIdAsync(string ingestionTrackingId) =>
            await ReadAsync<IngestionTracking>(ingestionTrackingId);

        public async ValueTask<IngestionTracking> UpdateIngestionTrackingnAsync(IngestionTracking ingestionTracking) =>
            await UpdateAsync(ingestionTracking);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await DeleteAsync(ingestionTracking);
    }
}
