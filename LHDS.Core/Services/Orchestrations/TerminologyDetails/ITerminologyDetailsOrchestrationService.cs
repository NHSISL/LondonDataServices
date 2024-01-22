// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.TerminologyDetails
{
    public interface ITerminologyDetailOrchestrationService
    {
        ValueTask RetrieveArtifactDetailsAsync();
    }
}