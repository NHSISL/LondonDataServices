// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Audit> Audits { get; set; }

        public async ValueTask<Audit> InsertAuditAsync(Audit audit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Audit> auditEntityEntry =
                await broker.Audits.AddAsync(audit);

            await broker.SaveChangesAsync();

            return auditEntityEntry.Entity;
        }

        public IQueryable<Audit> SelectAllAudits()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Audits;
        }

        public async ValueTask<Audit> SelectAuditByIdAsync(Guid auditId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Audits.FindAsync(auditId);
        }

        public async ValueTask<Audit> UpdateAuditAsync(Audit audit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Audit> auditEntityEntry =
                broker.Audits.Update(audit);

            await broker.SaveChangesAsync();

            return auditEntityEntry.Entity;
        }

        public async ValueTask<Audit> DeleteAuditAsync(Audit audit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Audit> auditEntityEntry =
                broker.Audits.Remove(audit);

            await broker.SaveChangesAsync();

            return auditEntityEntry.Entity;
        }
    }
}
