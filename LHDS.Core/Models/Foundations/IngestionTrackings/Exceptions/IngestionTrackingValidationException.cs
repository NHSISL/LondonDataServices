// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class IngestionTrackingValidationException : Xeption
    {
        public IngestionTrackingValidationException(Xeption innerException)
            : base(
                message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                innerException)
        { }
    }
}