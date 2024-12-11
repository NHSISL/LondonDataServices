// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class NotFoundIngestionTrackingException : Xeption
    {
        public NotFoundIngestionTrackingException(Guid ingestionTrackingId)
            : base(message: $"Couldn't find ingestion tracking with ingestionTrackingId: {ingestionTrackingId}.") { }

        public NotFoundIngestionTrackingException(string fileName)
            : base(message: $"Couldn't find ingestion tracking with file name: {fileName}.") { }
    }
}
