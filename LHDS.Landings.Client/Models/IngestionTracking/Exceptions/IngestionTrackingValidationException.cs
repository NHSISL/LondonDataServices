// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.IngestionTracking.Exceptions
{
    public class IngestionTrackingValidationException : Xeption
    {
        public IngestionTrackingValidationException(Xeption innerException)
            : base(message: "Document validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
