using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class IngestionTrackingServiceException : Xeption
    {
        public IngestionTrackingServiceException(Exception innerException)
            : base(message: "IngestionTracking service error occurred, contact support.", innerException)
        { }
    }
}