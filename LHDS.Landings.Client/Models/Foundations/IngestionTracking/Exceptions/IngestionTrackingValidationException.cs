// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions
{
    public class IngestionTrackingValidationException : Xeption
    {
        public IngestionTrackingValidationException(Xeption innerException)
            : base(message: "Ingestion tracking validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
