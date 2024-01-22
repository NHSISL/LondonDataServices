// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions
{
    public class InvalidArgumentIngestionTrackingAuditProcessingException : Xeption
    {
        public InvalidArgumentIngestionTrackingAuditProcessingException(string message)
            : base(message)
        { }
    }
}