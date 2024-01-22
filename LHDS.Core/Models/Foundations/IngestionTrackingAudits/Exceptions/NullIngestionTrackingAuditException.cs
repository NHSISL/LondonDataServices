// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class NullIngestionTrackingAuditException : Xeption
    {
        public NullIngestionTrackingAuditException(string message)
            : base(message)
        { }
    }
}