// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class IngestionTrackingValidationException : Xeption
    {
        public IngestionTrackingValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}