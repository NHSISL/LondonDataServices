// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSet>()
                .ToTable("DataSets", "Configuration");

            modelBuilder.Entity<DataSet>()
                .ToTable(dataSet => dataSet.IsTemporal());

            modelBuilder.Entity<DataSet>()
               .Property(dataSet => dataSet.Id)
               .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.SupplierId)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.DataSetName)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.DataSetAliases)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.DataSetAuthor)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.SpecifiedBy)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.IsNationallySpecified)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.CollectedBy)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.IsNationallyCollected)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.DataSourceType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.ActiveFrom)
                .IsRequired(false);

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.ActiveTo)
                .IsRequired(false);

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(dataSet => dataSet.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .HasIndex(dataSet => dataSet.DataSetName)
                .IsUnique();

            modelBuilder.Entity<DataSet>()
                .HasOne(dataSet => dataSet.Supplier)
                .WithMany(supplier => supplier.DataSets)
                .HasForeignKey(dataSet => dataSet.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}