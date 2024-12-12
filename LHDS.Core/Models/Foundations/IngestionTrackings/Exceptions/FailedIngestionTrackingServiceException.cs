// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingServiceException : Xeption
    {
        public FailedIngestionTrackingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
