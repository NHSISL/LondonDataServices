// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class NullIngestionTrackingException : Xeption
    {
        public NullIngestionTrackingException(string message)
            : base(message)
        { }
    }
}
