// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class IngestionTrackingProcessingDependencyException : Xeption
    {
        public IngestionTrackingProcessingDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}
