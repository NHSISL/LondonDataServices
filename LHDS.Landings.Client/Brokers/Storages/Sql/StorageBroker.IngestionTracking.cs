// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTracking> IngestionTrackings { get; set; }

        public async ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await InsertAsync(ingestionTracking);

        public IQueryable<IngestionTracking> SelectAllIngestionTracking() => ReadAll<IngestionTracking>();

        public async ValueTask<IngestionTracking> SelectIngestionTrackingByIdAsync(string ingestionTrackingId) =>
            await ReadAsync<IngestionTracking>(ingestionTrackingId);

        public async ValueTask<IngestionTracking> UpdateIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await UpdateAsync(ingestionTracking);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await DeleteAsync(ingestionTracking);
    }
}
