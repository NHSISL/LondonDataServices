using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddResolvedAddressAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResolvedAddressAudit>()
                .ToTable("ResolvedAddressAudit", "Addresses");

            modelBuilder.Entity<ResolvedAddressAudit>()
                .Property(ResolvedAddressAudit => ResolvedAddressAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddressAudit>()
                .Property(ResolvedAddressAudit => ResolvedAddressAudit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddressAudit>()
                .Property(ResolvedAddressAudit => ResolvedAddressAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddressAudit>()
                .Property(ResolvedAddressAudit => ResolvedAddressAudit.UpdatedDate)
                .IsRequired();
        }
    }
}
