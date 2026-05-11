// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<PdsAudit> PdsAudits { get; set; }

        public async ValueTask<PdsAudit> InsertPdsAuditAsync(
            PdsAudit pdsAudit,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(pdsAudit, cancellationToken);

        public async ValueTask<IQueryable<PdsAudit>> SelectAllPdsAuditsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<PdsAudit>(cancellationToken);

        public async ValueTask<PdsAudit> SelectPdsAuditByIdAsync(
            Guid pdsAuditId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<PdsAudit>(new object[] { pdsAuditId }, cancellationToken);

        public async ValueTask<PdsAudit> UpdatePdsAuditAsync(
            PdsAudit pdsAudit,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(pdsAudit, cancellationToken);

        public async ValueTask<PdsAudit> DeletePdsAuditAsync(
            PdsAudit pdsAudit,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(pdsAudit, cancellationToken);
    }
}
