// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using System.Reflection.Metadata;
using System;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.IngestionTracking;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public interface IIngestionTrackingService
    {
        ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<Document> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<Document> RetrieveAllIngestionTracking();
        ValueTask<Document> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
        ValueTask<Document> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<Document> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
    }


}
}