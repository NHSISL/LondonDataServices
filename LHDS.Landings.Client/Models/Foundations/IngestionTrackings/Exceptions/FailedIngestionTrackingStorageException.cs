// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions
{
    public class FailedIngestionTrackingStorageException : Xeption
    {
        public FailedIngestionTrackingStorageException(Exception innerException)
            : base(message: "Failed ingestion tracking storage error occurred, contact support.")
        { }
    }
}
