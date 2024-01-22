// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.Ontologies
{
    internal class OntologyAsset
    {
        public string FullUrl { get; set; }
        public string ResourceType { get; set; }
        public string? Version { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
    }
}
