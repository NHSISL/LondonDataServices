// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingProcessingServiceException : Xeption
    {
        public FailedIngestionTrackingProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
