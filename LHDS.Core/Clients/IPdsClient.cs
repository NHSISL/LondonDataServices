// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Clients
{
    public interface IPdsClient
    {
        ValueTask<PdsAudit> PickupFileAndSendToMesh(
            Stream pdsStream,
            string fileName,
            CancellationToken cancellationToken = default);

        ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage(
            CancellationToken cancellationToken = default);

        ValueTask<bool> ValidateMailboxAccessAsync(CancellationToken cancellationToken = default);
    }
}
