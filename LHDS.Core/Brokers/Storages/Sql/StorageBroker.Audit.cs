// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Audit> Audits { get; set; }

        public async ValueTask<Audit> InsertAuditAsync(Audit audit) =>
            await InsertAsync(audit);

        public IQueryable<Audit> SelectAllAudits() => SelectAll<Audit>();

        public async ValueTask<Audit> SelectAuditByIdAsync(Guid auditId) =>
            await SelectAsync<Audit>(auditId);

        public async ValueTask<Audit> UpdateAuditAsync(Audit audit) =>
            await UpdateAsync(audit);

        public async ValueTask<Audit> DeleteAuditAsync(Audit audit) =>
            await DeleteAsync(audit);
    }
}
