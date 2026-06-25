// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Orchestrations.AddressToUprns
{
    public class AddressToUprnCsv
    {
        public string AddressFormat { get; set; }
        public string PostcodeQuality { get; set; }
        public string IsMatched { get; set; }
        public string UPRN { get; set; }
        public string Qualifier { get; set; }
        public string Classification { get; set; }
        public string Algorithm { get; set; }
        public string AddressNumber { get; set; }
        public string AddressStreet { get; set; }
        public string AddressTown { get; set; }
        public string AddressPostcode { get; set; }
        public string AddressOrganization { get; set; }
        public string MatchPatternFlat { get; set; }
        public string MatchPatternBuilding { get; set; }
        public string MatchPatternNumber { get; set; }
        public string MatchPatternStreet { get; set; }
        public string MatchPatternPostCode { get; set; }
        public string CorrelationId { get; set; }
        public string UnstructuredAddress { get; set; }
    }
}
