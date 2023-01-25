// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.IngestionTracking.Exceptions
{
    public class IngestionTrackingServiceException : Xeption
    {
        public IngestionTrackingServiceException(Exception innerException)
            : base(message: "Ingestion tracking service error occurred, contact support.",
                  innerException)
        { }
    }
}
