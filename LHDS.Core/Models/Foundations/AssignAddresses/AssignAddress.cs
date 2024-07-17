// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignMatchPatterns;

namespace LHDS.Core.Models.Foundations.AssignAddresses
{
    public class AssignAddress
    {
        public string AddressFormat { get; set; }
        public string PostcodeQuality { get; set; }
        public bool Matched { get; set; }
        public long UPRN { get; set; }
        public string Qualifier { get; set; }
        public string Classification { get; set; }
        public string Algorithm { get; set; }
        public AssignABPAddress ABPAddress { get; set; }
        public AssignMatchPattern MatchPattern { get; set; }
    }
}
