// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions
{
    public class IngestionTrackingAuditProcessingValidationException : Xeption
    {
        public IngestionTrackingAuditProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
