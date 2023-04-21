// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public interface IOptOutOrchestrationService
    {
        ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName);
        ValueTask PushExpiredOptOutsToMeshForRenewalAsync();
        ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync();
    }
}
