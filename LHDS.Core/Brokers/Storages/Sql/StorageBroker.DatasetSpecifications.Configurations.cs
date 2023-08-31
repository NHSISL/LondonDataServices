// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSpecificationConfigurations(ModelBuilder modelBuilder)
        {
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
                .Property(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.IsMultiSender)
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
                .Property(dataSetSpecification => dataSetSpecification.SupersededBy)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(dataSetSpecification => dataSetSpecification.PresededBy)
                .HasMaxLength(255)
                .IsRequired();

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
        }
    }
}