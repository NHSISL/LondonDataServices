using LHDS.Core.Models.IngestionTrackings;
using LHDS.Core.Models.IngestionTrackings.Exceptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private void ValidateIngestionTrackingOnAdd(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);
        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking ingestionTracking)
        {
            if (ingestionTracking is null)
            {
                throw new NullIngestionTrackingException();
            }
        }
    }
}