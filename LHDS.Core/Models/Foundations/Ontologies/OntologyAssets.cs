// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace LHDS.Core.Models.Foundations.Ontologies
{
    internal class OntologyAssets
    {
        public List<OntologyAsset> Assets { get; set; } = new List<OntologyAsset>();
        public string? NextPage { get; set; } = string.Empty;
    }
}
