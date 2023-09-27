// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class IngestionTrackingDependencyValidationException : Xeption
    {
        public IngestionTrackingDependencyValidationException(string message, Xeption innerException)
            : base(message: "Ingestion tracking dependency validation occurred, please try again.", innerException) { }
    }
}
