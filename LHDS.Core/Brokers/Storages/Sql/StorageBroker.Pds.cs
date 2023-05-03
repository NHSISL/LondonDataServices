// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<PdsAudit> PdsAudits { get; set; }

        public async ValueTask<PdsAudit> InsertPdsAuditAsync(PdsAudit pdsAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<PdsAudit> pdsAuditEntityEntry =
                await broker.PdsAudits.AddAsync(pdsAudit);

            await broker.SaveChangesAsync();

            return pdsAuditEntityEntry.Entity;
        }

        public IQueryable<PdsAudit> SelectAllPdsAudits()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.PdsAudits;
        }

        public async ValueTask<PdsAudit> SelectPdsAuditByIdAsync(Guid pdsAuditId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.PdsAudits.FindAsync(pdsAuditId);
        }

        public async ValueTask<PdsAudit> UpdatePdsAuditAsync(PdsAudit pdsAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<PdsAudit> pdsAuditEntityEntry =
                broker.PdsAudits.Update(pdsAudit);

            await broker.SaveChangesAsync();

            return pdsAuditEntityEntry.Entity;
        }

        public async ValueTask<PdsAudit> DeletePdsAuditAsync(PdsAudit pdsAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<PdsAudit> pdsAuditEntityEntry =
                broker.PdsAudits.Remove(pdsAudit);

            await broker.SaveChangesAsync();

            return pdsAuditEntityEntry.Entity;
        }
    }
}
