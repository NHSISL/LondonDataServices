// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Clients
{
    public interface IOptOutClient
    {
        ValueTask RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName);
        ValueTask PushExpiredOptOutsToMeshForRenewalAsync();
        ValueTask RetrieveUpdatedMeshOptOutStatusChangesAsync();
    }
}
