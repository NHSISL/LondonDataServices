using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class AlreadyExistsIngestionTrackingException : Xeption
    {
        public AlreadyExistsIngestionTrackingException(Exception innerException)
            : base(message: "IngestionTracking with the same Id already exists.", innerException)
        { }
    }
}