// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Audit> Audits { get; set; }

        public async ValueTask BulkInsertAuditsAsync(
            List<Audit> audits,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkInsertAsync(audits, useTransaction, cancellationToken);

        public async ValueTask<Audit> InsertAuditAsync(
            Audit audit,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(audit, cancellationToken);

        public async ValueTask<IQueryable<Audit>> SelectAllAuditsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<Audit>(cancellationToken);

        public async ValueTask<Audit> SelectAuditByIdAsync(
            Guid auditId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<Audit>(new object[] { auditId }, cancellationToken);

        public async ValueTask<Audit> UpdateAuditAsync(
            Audit audit,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(audit, cancellationToken);

        public async ValueTask<Audit> DeleteAuditAsync(
            Audit audit,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(audit, cancellationToken);
    }
}
