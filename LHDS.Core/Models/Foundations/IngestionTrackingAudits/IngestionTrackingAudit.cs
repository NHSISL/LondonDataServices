// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LHDS.Core.Models.Foundations.IngestionTrackingAudits
{
    public class IngestionTrackingAudit : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid IngestionTrackingId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        [BindNever]
        public IngestionTracking? IngestionTracking { get; set; } = null!;
    }
}
