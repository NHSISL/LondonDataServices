// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class FailedIngestionTrackingAuditStorageException : Xeption
    {
        public FailedIngestionTrackingAuditStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}