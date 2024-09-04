// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public interface IIngestionTrackingProcessingService
    {
        ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<IngestionTracking> RetrieveAllIngestionTrackings();
        ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
        ValueTask<IngestionTracking> RetrieveOrAddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> ModifyOrAddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
        ValueTask<List<string>> RetrieveObjectsInBatchByBatchReference(string bacthReference);
    }
}
