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
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? XCoordinate { get; set; }
        public string? YCoordinate { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}
