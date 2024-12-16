// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingStorageException : Xeption
    {
        public FailedIngestionTrackingStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
