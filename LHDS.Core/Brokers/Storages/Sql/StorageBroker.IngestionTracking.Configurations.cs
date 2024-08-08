// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddIngestionTrackingConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IngestionTracking>()
                .ToTable("IngestionTrackings", "Ingestion");

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.FileName)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .HasIndex(ingestionTracking => ingestionTracking.FileName)
                .IsUnique();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.SupplierId)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.DataSetSpecificationId)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.EncryptedFileName)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.EncryptedFileSha256Hash)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.DecryptedFileName)
                .HasMaxLength(450)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.DecryptedFileSha256Hash)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .Property(ingestionTracking => ingestionTracking.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<IngestionTracking>()
                .HasOne(ingestionTracking => ingestionTracking.Supplier)
                .WithMany(supplier => supplier.IngestionTrackings)
                .HasForeignKey(ingestionTracking => ingestionTracking.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}