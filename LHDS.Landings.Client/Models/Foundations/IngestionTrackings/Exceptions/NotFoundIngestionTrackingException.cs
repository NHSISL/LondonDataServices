// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions
{
    public class NotFoundIngestionTrackingException : Xeption
    {
        public NotFoundIngestionTrackingException(Guid ingestionTrackingId)
            : base(message: $"Couldn't find ingestion tracking with ingestionTrackingId: {ingestionTrackingId}.")
        { }
    }
}
