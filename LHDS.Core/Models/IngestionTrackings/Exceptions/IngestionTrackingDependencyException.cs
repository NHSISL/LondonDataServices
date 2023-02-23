using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class IngestionTrackingDependencyException : Xeption
    {
        public IngestionTrackingDependencyException(Xeption innerException) :
            base(message: "IngestionTracking dependency error occurred, contact support.", innerException)
        { }
    }
}