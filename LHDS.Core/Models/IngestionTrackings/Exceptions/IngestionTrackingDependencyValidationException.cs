using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class IngestionTrackingDependencyValidationException : Xeption
    {
        public IngestionTrackingDependencyValidationException(Xeption innerException)
            : base(message: "IngestionTracking dependency validation occurred, please try again.", innerException)
        { }
    }
}