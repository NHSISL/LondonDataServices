// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Brokers.Decisions
{
    public class DecisionConfiguration
    {
        public string HashPepper { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string FilePrefix { get; set; } = string.Empty;
        public string IDecideBaseUrl { get; set; } = string.Empty;
        public string IDecidePatientDecisionsRelativeUrl { get; set; } = string.Empty;
        public string IDecideRecordAdoptionRelativeUrl { get; set; } = string.Empty;
        public string EntraTokenUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public int MaxResponseContentBufferSizeInMegaBytes { get; set; } = 400;
        public int TimeoutInSeconds { get; set; } = 600;
    }
}
