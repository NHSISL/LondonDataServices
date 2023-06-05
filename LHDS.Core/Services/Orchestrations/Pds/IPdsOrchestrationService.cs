// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public interface IPdsOrchestrationService
    {
        ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile);
        ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage();
    }
}
