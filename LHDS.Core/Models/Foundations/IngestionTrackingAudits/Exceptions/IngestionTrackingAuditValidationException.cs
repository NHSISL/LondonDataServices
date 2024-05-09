// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions
{
    public class IngestionTrackingAuditValidationException : Xeption
    {
        public IngestionTrackingAuditValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}