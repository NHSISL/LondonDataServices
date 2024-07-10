// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSupplierConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>()
                .ToTable("Suppliers", "Configuration");

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.Name)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.FriendlyName)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .Property(supplier => supplier.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<Supplier>()
                .HasIndex(supplier => supplier.Name)
                .IsUnique();
        }
    }
}