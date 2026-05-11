// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<PdsAudit> InsertPdsAuditAsync(PdsAudit pdsAudit, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<PdsAudit>> SelectAllPdsAuditsAsync(CancellationToken cancellationToken = default);
        ValueTask<PdsAudit> SelectPdsAuditByIdAsync(Guid pdsAuditId, CancellationToken cancellationToken = default);
        ValueTask<PdsAudit> UpdatePdsAuditAsync(PdsAudit pdsAudit, CancellationToken cancellationToken = default);
        ValueTask<PdsAudit> DeletePdsAuditAsync(PdsAudit pdsAudit, CancellationToken cancellationToken = default);
    }
}
