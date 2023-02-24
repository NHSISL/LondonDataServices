// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class AlreadyExistsIngestionTrackingException : Xeption
    {
        public AlreadyExistsIngestionTrackingException(Exception innerException)
            : base(message: "Ingestion tracking with the same Id already exists.") { }
    }
}
