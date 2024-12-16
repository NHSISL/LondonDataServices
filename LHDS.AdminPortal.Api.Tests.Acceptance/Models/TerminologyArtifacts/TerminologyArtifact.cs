// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts
{
    public class TerminologyArtifact
    {
        public Guid Id { get; set; }
        public string FullUrl { get; set; }
        public string ResourceType { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public bool IsCore { get; set; }
        public bool IsDownloaded { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
