// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions
{
    public class InvalidIngestionTrackingReferenceException : Xeption
    {
        public InvalidIngestionTrackingReferenceException(Exception innerException)
            : base(message: "Invalid ingestion tracking reference error occurred.", innerException)
        { }
    }
}
