// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Models.Audits
{
    public class Audit
    {
        public Guid Id { get; set; }
        public string IngestionTrackingId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public IngestionTracking IngestionTracking { get; set; }
    }
}
