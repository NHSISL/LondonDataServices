// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddResolvedAddressConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResolvedAddress>()
                .ToTable("ResolvedAddress", "Addresses");

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UniqueReference)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.PostCode)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UnstructuredPostalAddress)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.MatchedUPRN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.MatchedUPSN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.PostalAddress)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.JsonPostalAddress)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UpdatedDate)
                .IsRequired();
        }
    }
}
