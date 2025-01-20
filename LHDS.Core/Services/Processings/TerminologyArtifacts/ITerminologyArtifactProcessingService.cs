// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

namespace LHDS.Core.Services.Processings.TerminologyArtifacts
{
    public interface ITerminologyArtifactProcessingService
    {
        ValueTask<IQueryable<TerminologyArtifact>> RetrieveAllTerminologyArtifactsAsync();
        ValueTask<TerminologyArtifact> RetrieveTerminologyArtifactByIdAsync(Guid Id);
        ValueTask<TerminologyArtifact> ModifyOrAddTerminologyArtifactAsync(TerminologyArtifact terminologyArtifact);
        ValueTask<TerminologyArtifact> RemoveTerminologyArtifactByIdAsync(Guid Id);
        ValueTask<TerminologyArtifact?> GetNonDownloadedArtifactAsync();
    }
}
