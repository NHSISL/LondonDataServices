// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.AddressMatchers
{
    public class AddressMatch
    {
        public string? PostalAddress { get; set; }
        public string? JsonPostalAddress { get; set; }
        public string? UPRN { get; set; }
        public string? UPSN { get; set; }

        public IList<KeyValuePair<string, string>> NormalisedAddressComponents { get; set; }
            = new List<KeyValuePair<string, string>>();

        public IList<KeyValuePair<string, string>> OriginalAddressComponents { get; set; }
            = new List<KeyValuePair<string, string>>();

        public int MatchedComponents { get; set; } = 0;
        public bool MatchingCoreComponents { get; internal set; } = false;
        public bool AllNumbersMatch { get; set; } = false;
        public bool IsMatched { get; set; } = false;
        public BestMatchEnum BestMatch { get; set; } = BestMatchEnum.None;
    }
}
