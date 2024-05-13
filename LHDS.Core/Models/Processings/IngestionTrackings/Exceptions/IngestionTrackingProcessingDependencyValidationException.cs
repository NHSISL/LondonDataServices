// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class IngestionTrackingProcessingDependencyValidationException : Xeption
    {
        public IngestionTrackingProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
