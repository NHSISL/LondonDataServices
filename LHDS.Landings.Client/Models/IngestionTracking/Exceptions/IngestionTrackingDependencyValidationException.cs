// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.IngestionTracking.Exceptions
{
    public class IngestionTrackingDependencyValidationException : Xeption
    {
        public IngestionTrackingDependencyValidationException(Xeption innerException)
            : base(message: "Ingestion tracking dependency validation occurred, please try again.")
        { }
    }
}
