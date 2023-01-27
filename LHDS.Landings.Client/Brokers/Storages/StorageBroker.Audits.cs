using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LHDS.Landings.Client.Models.Audits;

namespace LHDS.Landings.Client.Brokers.Storages
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
    }
}
