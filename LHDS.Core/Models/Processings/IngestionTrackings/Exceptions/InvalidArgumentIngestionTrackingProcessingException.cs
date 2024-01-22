// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class InvalidArgumentIngestionTrackingProcessingException : Xeption
    {
        public InvalidArgumentIngestionTrackingProcessingException(string message)
            : base(message)
        { }
    }
}