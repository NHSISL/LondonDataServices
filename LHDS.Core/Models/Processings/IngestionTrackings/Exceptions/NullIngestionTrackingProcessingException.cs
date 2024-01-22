// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class NullIngestionTrackingProcessingException : Xeption
    {
        public NullIngestionTrackingProcessingException(string message)
            : base(message)
        { }
    }
}
