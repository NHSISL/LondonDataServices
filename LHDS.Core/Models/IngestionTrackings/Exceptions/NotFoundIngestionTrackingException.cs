using System;
using Xeptions;

namespace LHDS.Core.Models.IngestionTrackings.Exceptions
{
    public class NotFoundIngestionTrackingException : Xeption
    {
        public NotFoundIngestionTrackingException(Guid ingestionTrackingId)
            : base(message: $"Couldn't find ingestionTracking with ingestionTrackingId: {ingestionTrackingId}.")
        { }
    }
}