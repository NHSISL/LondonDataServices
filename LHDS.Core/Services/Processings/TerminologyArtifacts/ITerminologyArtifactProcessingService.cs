// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    internal interface ITerminologyArtifactProcessingService
    {
        IQueryable<TerminologyArtifact> RetrieveAllTerminologyArtifactsAsync();
        ValueTask<TerminologyArtifact> RetrieveAllTerminologyArtifactByIdAsync(Guid Id);
        ValueTask<TerminologyArtifact> RetrieveOrAddTerminologyArtifactAsync(OntologyAsset ontologyAsset);
        ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id);
    }
}
