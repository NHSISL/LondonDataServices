// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AssignAddresses.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignMatchPatterns;
using Newtonsoft.Json;

namespace LHDS.Core.Models.Foundations.AssignAddresses.BestMatch
{
    public class BestMatch
    {
        public string UPRN { get; set; }
        public string Qualifier { get; set; }
        public string Classification { get; set; }
        public string Algorithm { get; set; }
        public AssignABPAddress ABPAddress { get; set; }

        [JsonProperty("Match_pattern")]
        public AssignMatchPattern MatchPattern { get; set; }
    }
}
