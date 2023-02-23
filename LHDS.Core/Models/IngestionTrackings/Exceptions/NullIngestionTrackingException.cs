using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class NullIngestionTrackingException : Xeption
    {
        public NullIngestionTrackingException()
            : base(message: "IngestionTracking is null.")
        { }
    }
}