using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingStorageException : Xeption
    {
        public FailedIngestionTrackingStorageException(Exception innerException)
            : base(message: "Failed ingestionTracking storage error occurred, contact support.", innerException)
        { }
    }
}