using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class InvalidIngestionTrackingReferenceException : Xeption
    {
        public InvalidIngestionTrackingReferenceException(Exception innerException)
            : base(message: "Invalid ingestionTracking reference error occurred.", innerException) { }
    }
}