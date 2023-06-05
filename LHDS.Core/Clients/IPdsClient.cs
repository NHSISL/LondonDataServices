// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Clients
{
    public interface IPdsClient
    {
        ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile);
        ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage();
    }
}
