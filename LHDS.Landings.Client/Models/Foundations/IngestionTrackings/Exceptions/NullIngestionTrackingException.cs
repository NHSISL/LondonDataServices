// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions
{
    public class NullIngestionTrackingException : Xeption
    {
        public NullIngestionTrackingException()
            : base(message: "Ingestion tracking is null.")
        { }
    }
}
