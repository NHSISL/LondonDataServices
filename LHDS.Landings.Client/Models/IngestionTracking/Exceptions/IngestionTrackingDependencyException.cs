// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.IngestionTracking.Exceptions
{
    public class IngestionTrackingDependencyException : Xeption
    {
        public IngestionTrackingDependencyException(Xeption innerException) 
            : base(message: "Ingestion tracking dependency error occurred, contact support.")
        { }
    }
}
