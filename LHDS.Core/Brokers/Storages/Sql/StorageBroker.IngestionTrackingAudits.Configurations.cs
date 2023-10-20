// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddIngestionTrackingAuditConfigurations(ModelBuilder modelBuilder)
        {
            // Ingestion
            modelBuilder.Entity<IngestionTrackingAudit>()
                .ToTable("IngestionTrackingAudits", "Ingestion");

            modelBuilder.Entity<IngestionTrackingAudit>()
                .Property(ingestionTrackingAudit => ingestionTrackingAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<IngestionTrackingAudit>()
                .Property(ingestionTrackingAudit => ingestionTrackingAudit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<IngestionTrackingAudit>()
                .Property(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<IngestionTrackingAudit>()
                .Property(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<IngestionTrackingAudit>()
                .Property(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTrackingId)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTrackingAudit>()
                .HasOne(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTracking)
                .WithMany(ingestionTracking => ingestionTracking.IngestionTrackingAudits)
                .HasForeignKey(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTrackingId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
