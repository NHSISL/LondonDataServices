// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Audits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>()
                .Property(audit => audit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.IngestionTrackingId)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .HasOne(audit => audit.IngestionTracking)
                .WithMany(ingestionTracking => ingestionTracking.Audits)
                .HasForeignKey(audit => audit.IngestionTrackingId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
