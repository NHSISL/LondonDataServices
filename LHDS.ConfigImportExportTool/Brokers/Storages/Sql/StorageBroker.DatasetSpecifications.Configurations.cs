// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSpecificationConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetSpecification>()
                .ToTable("DataSetSpecifications", "Configuration");

            modelBuilder.Entity<DataSetSpecification>()
                .ToTable(dataSet => dataSet.IsTemporal());

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.Id)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.DataSetId)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .HasIndex(dataSet => new { dataSet.DataSetId, dataSet.SupplierSpecificationVersion })
                .IsUnique();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .HasIndex(dataSet => new { dataSet.DataSetId, dataSet.OurSpecificationVersion })
                .IsUnique();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.IsMultiAuthorPerBatch)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.DateReleased)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.DateImplemented)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.DateSuperseded)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.SupersededById)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.PresededById)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.IsPublished)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .HasOne(columnDefinition => columnDefinition.DataSet)
                .WithMany(schemaDefinition => schemaDefinition.DataSetSpecifications)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DataSetSpecification>()
                .HasIndex(ds => ds.SupersededById)
                .IsUnique(false);

            modelBuilder.Entity<DataSetSpecification>()
                .HasIndex(ds => ds.PresededById)
                .IsUnique(false);

            modelBuilder.Entity<DataSetSpecification>()
                .HasMany(ds => ds.SupersededBy)
                .WithOne()
                .HasForeignKey(ds => ds.SupersededById)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .HasMany(ds => ds.PresededBy)
                .WithOne()
                .HasForeignKey(ds => ds.PresededById)
                .IsRequired(false);
        }
    }
}