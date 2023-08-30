// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSetObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetObjectConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetObject>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.Id)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.DataSetSpecificationId)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.SupplierObjectName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.OurObjectName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.ObjectDescription)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.InterchangeProtocol)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.PushOrPull)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.DeletionHandling)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.IsSubmissionHeaderObject)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.IsTransactionLog)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .Property(objectColumn => objectColumn.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSetObject>()
                .HasOne(columnDefinition => columnDefinition.DataSetSpecification)
                .WithMany(schemaDefinition => schemaDefinition.DataSetObjects)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetSpecificationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}