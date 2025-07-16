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
                .Property(address => address.UnstructuredPostalAddress)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
               .Property(address => address.AlternateUnstructuredPostalAddress)
               .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UPRN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.UPSN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.OrganisationName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.DepartmentName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.SubBuildingName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.BuildingName)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.BuildingNumber)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.DependentThoroughfare)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.Thoroughfare)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.DoubleDependentLocality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.DependentLocality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.PostTown)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.PostCode)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.AddressFormatQuality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.PostCodeQuality)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.MatchedWithAssign)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                 .Property(address => address.IsProcessed)
                 .HasDefaultValue(false)
                 .IsRequired();

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.Qualifier)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.Classification)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.Algorithm)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.MatchPattern)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
               .Property(address => address.Latitude)
               .HasMaxLength(255)
               .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
               .Property(address => address.Longitude)
               .HasMaxLength(255)
               .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
                .Property(address => address.XCoordinate)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ResolvedAddress>()
               .Property(address => address.YCoordinate)
               .HasMaxLength(255)
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
