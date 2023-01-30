// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public interface IIngestionTrackingService
    {
        ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<IngestionTracking> RetrieveAllIngestionTracking();
        ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(string ingestionTrackingId);
        ValueTask<IngestionTracking> RetrieveIngestionTrackingByFileNameAsync(string fileName);
        ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(string ingestionTrackingId);
    }
}