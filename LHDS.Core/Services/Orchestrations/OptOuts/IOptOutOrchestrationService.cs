// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public interface IOptOutOrchestrationService
    {
        ValueTask<string> RetrieveOptOutStatusAsync(Stream input, string fileName, CancellationToken cancellationToken);
        ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync(CancellationToken cancellationToken);
        ValueTask<List<MeshMessage>> RetrieveUpdatedMeshConsentStatusesChangesAsync(CancellationToken cancellationToken);
        ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken);
    }
}
