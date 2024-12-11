// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class InvalidIngestionTrackingException : Xeption
    {
        public InvalidIngestionTrackingException(string message)
            : base(message)
        { }
    }
}
