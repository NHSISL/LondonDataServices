// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAddressLoadingAuditConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressLoadingAudit>()
                .ToTable("AddressLoadingAudit", "UPRN");

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.Id)
                .IsRequired();

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.CorrelationId)
                .IsRequired();

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.FileName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.Message)
                .IsRequired(false);

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<AddressLoadingAudit>()
                .Property(addressLoadingAudit => addressLoadingAudit.UpdatedDate)
                .IsRequired();
        }
    }
}
