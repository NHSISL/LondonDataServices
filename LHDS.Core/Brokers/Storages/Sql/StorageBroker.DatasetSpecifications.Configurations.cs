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
                .Property(objectColumn => objectColumn.Id)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.DataSetId)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.SupplierSpecificationVersion)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.OurSpecificationVersion)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.IsMultiSender)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.DateReleased)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.DateImplemented)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.DateSuperseded)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.SupersededBy)
                .IsRequired(false);

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.PresededBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.IsPublished)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .Property(objectColumn => objectColumn.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetSpecification>()
                .HasOne(columnDefinition => columnDefinition.DataSet)
                .WithMany(schemaDefinition => schemaDefinition.DataSetSpecifications)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}