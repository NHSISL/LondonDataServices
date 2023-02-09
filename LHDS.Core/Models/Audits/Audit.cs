// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.Core.Models.Audits
{
    public class Audit
    {
        public Guid Id { get; set; }
        public string IngestionTrackingId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
