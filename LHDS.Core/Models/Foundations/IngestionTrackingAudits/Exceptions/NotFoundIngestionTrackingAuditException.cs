// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class NotFoundIngestionTrackingAuditException : Xeption
    {
        public NotFoundIngestionTrackingAuditException(Guid ingestionTrackingAuditId)
            : base(message: $"Couldn't find IngestionTrackingAudit with Id: {ingestionTrackingAuditId}.")
        { }
    }
}