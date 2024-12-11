// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSpecificationObjectConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecificationObject>()
                .ToTable("SpecificationObjects", "Configuration");

            modelBuilder.Entity<SpecificationObject>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.Id)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.DataSetSpecificationId)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.SupplierObjectName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.OurObjectName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.ObjectDescription)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.InterchangeProtocol)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsPushedToUs)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsPulledByUs)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.DeletionHandling)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsSubmissionHeaderObject)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsTransactionLog)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsCaseSensitive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsNumerice)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.IsPostcode)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.DeleteCondition)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .Property(objectColumn => objectColumn.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<SpecificationObject>()
                .HasOne(columnDefinition => columnDefinition.DataSetSpecification)
                .WithMany(schemaDefinition => schemaDefinition.SpecificationObjects)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetSpecificationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}