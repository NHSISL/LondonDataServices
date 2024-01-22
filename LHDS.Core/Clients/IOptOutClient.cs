// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Clients
{
    public interface IOptOutClient
    {
        ValueTask<string> RetrieveOptOutStatusAsync(byte[] optOutFile, string fileName);
        ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync();
        ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync();
        ValueTask<bool> ValidateMailboxAccessAsync();
    }
}
