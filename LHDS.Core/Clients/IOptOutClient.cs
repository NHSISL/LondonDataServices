// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Clients
{
    public interface IOptOutClient
    {
        ValueTask<string> RetrieveOptOutStatusAsync(
            Stream input,
            string fileName,
            CancellationToken cancellationToken = default);

        ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync(CancellationToken cancellationToken = default);

        ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync(
            CancellationToken cancellationToken = default);

        ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default);
    }
}
