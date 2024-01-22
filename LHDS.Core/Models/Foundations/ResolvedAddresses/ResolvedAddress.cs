// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses
{
    internal class ResolvedAddress : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string? UPRN { get; set; }
        public string? UPSN { get; set; }
        public string? PostCode { get; set; }
        public string? PostalAddress { get; set; }
        public string? JsonPostalAddress { get; set; }
        public MatchAlgorithmEnum MatchAlgorithmEnum { get; set; }
        public bool IsMatched { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
