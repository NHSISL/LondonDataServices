using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class IngestionTrackingValidationException : Xeption
    {
        public IngestionTrackingValidationException(Xeption innerException)
            : base(message: "IngestionTracking validation errors occurred, please try again.",
                  innerException)
        { }
    }
}