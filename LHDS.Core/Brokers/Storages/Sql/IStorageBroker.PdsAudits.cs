// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<PdsAudit> InsertPdsAuditAsync(PdsAudit pdsAudit);
        ValueTask<IQueryable<PdsAudit>> SelectAllPdsAuditsAsync();
        ValueTask<PdsAudit> SelectPdsAuditByIdAsync(Guid pdsAuditId);
        ValueTask<PdsAudit> UpdatePdsAuditAsync(PdsAudit pdsAudit);
        ValueTask<PdsAudit> DeletePdsAuditAsync(PdsAudit pdsAudit);
    }
}
