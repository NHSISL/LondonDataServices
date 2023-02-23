using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public interface IIngestionTrackingService
    {
        ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking);
        IQueryable<IngestionTracking> RetrieveAllIngestionTrackings();
        ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
        ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking);
        ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId);
    }
}