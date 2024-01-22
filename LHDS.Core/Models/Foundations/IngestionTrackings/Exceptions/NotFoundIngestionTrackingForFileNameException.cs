// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions
{
    public class NotFoundIngestionTrackingForFileNameException : Xeption
    {
        public NotFoundIngestionTrackingForFileNameException(string fileName)
            : base(message: $"Couldn't find ingestion tracking with fileName: {fileName}.") { }
    }
}
