// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace LHDS.Core.Models.Foundations.AssignAddresses
{
    public class AssignAddress
    {
        [JsonProperty("Address_format")]
        public string AddressFormat { get; set; }

        [JsonProperty("Postcode_quality")]
        public string PostcodeQuality { get; set; }

        public bool Matched { get; set; }
        public BestMatch.BestMatch BestMatch { get; set; }

        public string Pattern
        {
            get
            {
                if (BestMatch.MatchPattern != null)
                {
                    return
                        (!String.IsNullOrWhiteSpace(BestMatch.MatchPattern.Flat)
                            ? $"Flat: {BestMatch.MatchPattern.Flat}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(BestMatch.MatchPattern.Building)
                            ? $"Building: {BestMatch.MatchPattern.Building}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(BestMatch.MatchPattern.Number)
                            ? $"Number: {BestMatch.MatchPattern.Number}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(BestMatch.MatchPattern.Street)
                            ? $"Street: {BestMatch.MatchPattern.Street}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(BestMatch.MatchPattern.Postcode)
                            ? $"Postcode: {BestMatch.MatchPattern.Postcode}"
                            : "");
                };

                return string.Empty;
            }
        }
    }
}
