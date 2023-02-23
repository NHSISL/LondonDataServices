using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class LockedIngestionTrackingException : Xeption
    {
        public LockedIngestionTrackingException(Exception innerException)
            : base(message: "Locked ingestionTracking record exception, please try again later", innerException)
        {
        }
    }
}