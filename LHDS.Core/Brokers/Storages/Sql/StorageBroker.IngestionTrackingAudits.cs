// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTrackingAudit> IngestionTrackingAudits { get; set; }

        public async ValueTask<IngestionTrackingAudit> InsertIngestionTrackingAuditAsync(IngestionTrackingAudit ingestionTrackingAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<IngestionTrackingAudit> ingestionTrackingAuditEntityEntry =
                await broker.IngestionTrackingAudits.AddAsync(ingestionTrackingAudit);

            await broker.SaveChangesAsync();

            return ingestionTrackingAuditEntityEntry.Entity;
        }

        public IQueryable<IngestionTrackingAudit> SelectAllIngestionTrackingAudits()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.IngestionTrackingAudits;
        }

        public async ValueTask<IngestionTrackingAudit> SelectIngestionTrackingAuditByIdAsync(Guid ingestionTrackingAuditId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.IngestionTrackingAudits.FindAsync(ingestionTrackingAuditId);
        }

        public async ValueTask<IngestionTrackingAudit> UpdateIngestionTrackingAuditAsync(IngestionTrackingAudit ingestionTrackingAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<IngestionTrackingAudit> ingestionTrackingAuditEntityEntry =
                broker.IngestionTrackingAudits.Update(ingestionTrackingAudit);

            await broker.SaveChangesAsync();

            return ingestionTrackingAuditEntityEntry.Entity;
        }

        public async ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditAsync(IngestionTrackingAudit ingestionTrackingAudit)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<IngestionTrackingAudit> ingestionTrackingAuditEntityEntry =
                broker.IngestionTrackingAudits.Remove(ingestionTrackingAudit);

            await broker.SaveChangesAsync();

            return ingestionTrackingAuditEntityEntry.Entity;
        }
    }
}
