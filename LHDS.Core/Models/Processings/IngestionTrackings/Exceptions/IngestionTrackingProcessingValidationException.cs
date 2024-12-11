// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class IngestionTrackingProcessingValidationException : Xeption
    {
        public IngestionTrackingProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
