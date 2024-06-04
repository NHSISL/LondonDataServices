// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddAddressConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .ToTable("Address", "UPRN");

            modelBuilder.Entity<Address>()
                .Property(address => address.UPRN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .HasIndex(address => address.UPRN);

            modelBuilder.Entity<Address>()
                .Property(address => address.UPSN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.OrganisationName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.DepartmentName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.SubBuildingName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.BuildingName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.BuildingNumber)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.DependentThoroughfare)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.Thoroughfare)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.DoubleDependentLocality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.DependentLocality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.PostTown)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.PostCode)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.PostalAddress)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.JsonPostalAddress)
                .IsRequired(false);

            modelBuilder.Entity<Address>()
                .Property(address => address.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(address => address.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(address => address.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(address => address.UpdatedDate)
                .IsRequired();
        }
    }
}
