// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Brokers.Ontologies
{
    public class OntologyConfiguration
    {
        public string TerminologyServerBaseUrl { get; set; } = string.Empty;
        public string TerminologyServerAuthenticationRelativeUrl { get; set; } = string.Empty;
        public string TerminologyServerCodeSystemRelativeUrl { get; set; } = string.Empty;
        public string TerminologyServerValueSetRelativeUrl { get; set; } = string.Empty;
        public string TerminologyServerConceptMapRelativeUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
