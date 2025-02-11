// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class NotFoundIngestionTrackingAuditException : Xeption
    {
        public NotFoundIngestionTrackingAuditException(string message)
            : base(message)
        { }
    }
}