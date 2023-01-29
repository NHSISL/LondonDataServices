// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions
{
    public class NotFoundIngestionTrackingForFileNameException : Xeption
    {
        public NotFoundIngestionTrackingForFileNameException(string fileName)
            : base(message: $"Couldn't find ingestion tracking with fileName: {fileName}.")
        { }
    }
}
