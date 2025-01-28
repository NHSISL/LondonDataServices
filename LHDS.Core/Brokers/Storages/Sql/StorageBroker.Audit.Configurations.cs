// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Audits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>()
                .ToTable("Audits", "Audit");

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.Id)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.CorrelationId)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.AuditType)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.LogLevel)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.FileName)
                .HasMaxLength(1000)
                .IsRequired(false);

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.Title)
                .IsRequired(false);

            modelBuilder.Entity<Audit>()
                .Property(audit => audit.Message)
                .IsRequired(false);

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
                .HasIndex(audit => audit.CorrelationId);

            modelBuilder.Entity<Audit>()
                .HasIndex(audit => audit.AuditType);

            modelBuilder.Entity<Audit>()
                .HasIndex(audit => audit.LogLevel);
        }
    }
}
