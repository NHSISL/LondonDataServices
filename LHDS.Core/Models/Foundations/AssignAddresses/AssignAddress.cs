// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;
using LHDS.Core.Models.Foundations.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignMatchPatterns;

namespace LHDS.Core.Models.Foundations.AssignAddresses
{
    public class AssignAddress : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string AddressFormat { get; set; }
        public string PostcodeQuality { get; set; }
        public bool Matched { get; set; }
        public long UPRN { get; set; }
        public string Qualifier { get; set; }
        public string Classification { get; set; }
        public string Algorithm { get; set; }
        public AssignABPAddress ABPAddress { get; set; }
        public AssignMatchPattern MatchPattern { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
