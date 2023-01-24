// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.IngestionTracking;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Landings.Client.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTracking> IngestionTrackings { get; set; }

        public async ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await InsertAsync(ingestionTracking);

        public IQueryable<IngestionTracking> ReadAllIngestionTracking() => ReadAll<IngestionTracking>();

        public async ValueTask<IngestionTracking> ReadIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await ReadAsync<IngestionTracking>(ingestionTrackingId);
    }
}
