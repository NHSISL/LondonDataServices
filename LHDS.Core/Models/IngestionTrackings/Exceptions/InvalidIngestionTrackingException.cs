using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class InvalidIngestionTrackingException : Xeption
    {
        public InvalidIngestionTrackingException()
            : base(message: "Invalid ingestionTracking. Please correct the errors and try again.")
        { }
    }
}