// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignMatchPatterns;

namespace LHDS.Core.Models.Foundations.AssignAddresses.BestMatch
{
    public class BestMatch
    {
        public AssignABPAddress ABPAddress { get; set; }
        public AssignMatchPattern MatchPattern { get; set; }
    }
}
