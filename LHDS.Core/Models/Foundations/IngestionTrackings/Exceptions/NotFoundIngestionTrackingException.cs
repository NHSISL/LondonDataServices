// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class NotFoundIngestionTrackingException : Xeption
    {
        public NotFoundIngestionTrackingException(string ingestionTrackingId)
            : base(message: $"Couldn't find ingestion tracking with ingestionTrackingId: {ingestionTrackingId}.") { }
    }
}
