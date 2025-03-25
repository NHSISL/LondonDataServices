// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.StreetDescriptors;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddStreetDescriptorConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .ToTable("StreetDescriptor", "StreetDescriptors");

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.USRN)
                .HasMaxLength(15)
                .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.StreetDescription)
                .HasMaxLength(255)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.Locality)
                .HasMaxLength(255)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.Town)
                .HasMaxLength(255)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.AdminisatrativeArea)
                .HasMaxLength(255)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
               .Property(streetDescriptor => streetDescriptor.StartDate)
                .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.EndDate)
                .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.LastUpdatedDate)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.EntryDate)
                 .IsRequired(false);

            modelBuilder.Entity<StreetDescriptor>()
                .HasIndex(streetDescriptor => streetDescriptor.IsSynced);

            modelBuilder.Entity<StreetDescriptor>()
                .HasIndex(streetDescriptor => streetDescriptor.IsProcessing);

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<StreetDescriptor>()
                .Property(streetDescriptor => streetDescriptor.UpdatedDate)
                .IsRequired();
        }
    }
}
