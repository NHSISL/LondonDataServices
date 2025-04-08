// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Orchestrations.Addresses
{
    public class BLPUAddress
    {
        public string? UPRN { get; set; }
        public string? PostCode { get; set; }
        public int? LogicalStatus { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
