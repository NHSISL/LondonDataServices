// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddPdsAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PdsAudit>()
                .ToTable("Pds", "Patient");

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.FileName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.FileName)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<PdsAudit>()
                .Property(pdsAudit => pdsAudit.UpdatedDate)
                .IsRequired();
        }
    }
}
