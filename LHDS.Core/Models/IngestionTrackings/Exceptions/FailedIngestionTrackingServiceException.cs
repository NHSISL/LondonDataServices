using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingServiceException : Xeption
    {
        public FailedIngestionTrackingServiceException(Exception innerException)
            : base(message: "Failed ingestionTracking service occurred, please contact support", innerException)
        { }
    }
}