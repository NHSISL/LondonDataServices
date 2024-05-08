// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public interface ITerminologyMetadataOrchestrationService
    {
        ValueTask RetrieveArtifactMetadataAsync(string[] resourceTypes);
    }
}