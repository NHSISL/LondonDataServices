// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class LockedIngestionTrackingAuditException : Xeption
    {
        public LockedIngestionTrackingAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}