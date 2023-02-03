// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<IngestionTracking> ReadAllIngestionTracking();
        ValueTask<IngestionTracking> ReadIngestionTrackingByIdAsync(string ingestionTrackingId);
        ValueTask<IngestionTracking> ReadIngestionTrackingByFileNameAsync(string fileName);
        ValueTask<IngestionTracking> UpdateIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(IngestionTracking ingestionTracking);
    }
}
