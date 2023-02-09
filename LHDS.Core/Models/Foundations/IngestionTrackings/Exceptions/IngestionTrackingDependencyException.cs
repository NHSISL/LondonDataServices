// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class IngestionTrackingDependencyException : Xeption
    {
        public IngestionTrackingDependencyException(Xeption innerException)
            : base(message: "Ingestion tracking dependency error occurred, contact support.")
        { }
    }
}
