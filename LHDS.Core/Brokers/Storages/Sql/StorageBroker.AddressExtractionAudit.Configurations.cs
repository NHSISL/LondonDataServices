// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAddressExtractionAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressExtractionAudit>()
                .ToTable("AddressExtractionAudit", "UPRN");

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.Id)
                .IsRequired();

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.CorrelationId)
                .IsRequired();

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.FileName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.Message)
                .IsRequired(false);

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressExtractionAudit>()
                .Property(addressExtractionAudit => addressExtractionAudit.UpdatedDate)
                .IsRequired();
        }
    }
}
