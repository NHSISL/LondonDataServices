// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    internal interface ITerminologyArtifactProcessingService
    {
        ValueTask<TerminologyArtifact> RetrieveAllTerminologyArtifactsAsync(OntologyAsset ontologyAsset);
    }
}
