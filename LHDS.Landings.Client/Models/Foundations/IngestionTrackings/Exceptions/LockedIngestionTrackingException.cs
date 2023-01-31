// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NEL.Premises.Api.Models.Documents.Exceptions
{
    public class LockedIngestionTrackingException : Xeption
    {
        public LockedIngestionTrackingException(Exception innerException)
            : base(message: "Locked ingestion tracking record exception, please try again later", innerException)
        {
        }
    }
}