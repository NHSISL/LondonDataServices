// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.AssignABPAddresses;
using LHDS.Core.Models.Foundations.AssignAddresses.AssignMatchPatterns;

namespace LHDS.Core.Models.Foundations.AssignAddresses
{
    public class AssignAddress
    {
        public string AddressFormat { get; set; }
        public string PostcodeQuality { get; set; }
        public bool Matched { get; set; }
        public string UPRN { get; set; }
        public string Qualifier { get; set; }
        public string Classification { get; set; }
        public string Algorithm { get; set; }
        public AssignABPAddress ABPAddress { get; set; }
        public AssignMatchPattern MatchPattern { get; set; }

        public string Pattern
        {
            get
            {
                if (MatchPattern != null)
                {
                    return
                        (!String.IsNullOrWhiteSpace(MatchPattern.Flat)
                            ? $"Flat: {MatchPattern.Flat}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(MatchPattern.Building)
                            ? $"Building: {MatchPattern.Building}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(MatchPattern.Number)
                            ? $"Number: {MatchPattern.Number}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(MatchPattern.Street)
                            ? $"Street: {MatchPattern.Street}, "
                            : "") +

                        (!String.IsNullOrWhiteSpace(MatchPattern.Postcode)
                            ? $"Postcode: {MatchPattern.Postcode}"
                            : "");
                };

                return string.Empty;
            }
        }
    }
}
