// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class InvalidIngestionTrackingAuditException : Xeption
    {
        public InvalidIngestionTrackingAuditException(string message)
            : base(message) { }
    }
}