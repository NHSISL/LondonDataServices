// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions
{
    public class FailedIngestionTrackingAuditProcessingServiceException : Xeption
    {
        public FailedIngestionTrackingAuditProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
