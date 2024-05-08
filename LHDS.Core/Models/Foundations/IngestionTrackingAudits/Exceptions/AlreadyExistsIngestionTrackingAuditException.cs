// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class AlreadyExistsIngestionTrackingAuditException : Xeption
    {
        public AlreadyExistsIngestionTrackingAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}