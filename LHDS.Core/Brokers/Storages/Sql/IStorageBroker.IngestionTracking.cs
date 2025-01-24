// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<IngestionTracking> InsertIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IQueryable<IngestionTracking>> SelectAllIngestionTrackingsAsync();
        ValueTask<IngestionTracking> SelectIngestionTrackingByIdAsync(Guid ingestionTrackingId);
        ValueTask<IngestionTracking> UpdateIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(IngestionTracking ingestionTracking);
    }
}