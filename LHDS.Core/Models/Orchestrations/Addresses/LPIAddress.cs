// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Orchestrations.Addresses
{
    public class LPIAddress : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string? UPRN { get; set; }
        public int? LogicalStatus { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? SAOStartNumber { get; set; }
        public string? SAOStartSuffix { get; set; }
        public string? SAOEndNumber { get; set; }
        public string? SAOEndSuffix { get; set; }
        public string? SAOText { get; set; }
        public string? PAOStartNumber { get; set; }
        public string? PAOStartSuffix { get; set; }
        public string? PAOEndNumber { get; set; }
        public string? PAOEndSuffix { get; set; }
        public string? PAOText { get; set; }
        public string? USRN { get; set; }
        public bool IsProcessing { get; set; }
        public bool IsSynced { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
