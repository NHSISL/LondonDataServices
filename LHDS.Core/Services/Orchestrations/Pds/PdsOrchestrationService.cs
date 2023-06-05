// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationService : IPdsOrchestrationService
    {
        public ValueTask<PdsAudit> PickupFileAndSendToMesh(byte[] pdsFile, string fileName) =>
            throw new NotImplementedException();

        public ValueTask<List<PdsAudit>> RetreiveMessagesFromMeshAndUpdateStorage() =>
            throw new NotImplementedException();
    }
}
