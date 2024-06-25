// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Brokers.Ontologies
{
    public class OntologyConfiguration
    {
        public string TerminologyServerBaseUrl { get; set; } = string.Empty;
        public string TerminologyServerAuthenticationRelativeUrl { get; set; } = string.Empty;
        public string TerminologyServerResourceRelativeUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public int MaxResponseContentBufferSizeInMegaBytes { get; set; } = 384;
        public int TimeoutInSeconds { get; set; } = 600;
    }
}
