// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public interface IIngestionTrackingService
    {
        ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<IngestionTracking> RetrieveAllIngestionTrackings();
        ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(string ingestionTrackingId);
        ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(string ingestionTrackingId);
    }
}