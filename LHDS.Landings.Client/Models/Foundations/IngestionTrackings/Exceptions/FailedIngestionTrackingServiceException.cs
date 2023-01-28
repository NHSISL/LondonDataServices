// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingServiceException : Xeption
    {
        public FailedIngestionTrackingServiceException(Exception innerException)
            : base(message: "Failed ingestion tracking service occurred, please contact support", innerException)
        { }
    }
}
