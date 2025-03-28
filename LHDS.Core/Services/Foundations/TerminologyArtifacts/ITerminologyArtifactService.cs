// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public interface ITerminologyArtifactService
    {
        ValueTask<TerminologyArtifact> AddTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact);
        ValueTask<IQueryable<TerminologyArtifact>> RetrieveAllTerminologyArtifactsAsync();
        ValueTask<TerminologyArtifact> RetrieveTerminologyArtifactByIdAsync(Guid terminologyArtifactId);
        ValueTask<TerminologyArtifact> ModifyTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact);
        ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid terminologyArtifactId);
    }
}