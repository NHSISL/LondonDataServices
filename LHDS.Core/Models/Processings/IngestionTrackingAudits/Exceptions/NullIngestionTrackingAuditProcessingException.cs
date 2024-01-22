// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions
{
    public class NullIngestionTrackingAuditProcessingException : Xeption
    {
        public NullIngestionTrackingAuditProcessingException(string message)
            : base(message)
        { }
    }
}
