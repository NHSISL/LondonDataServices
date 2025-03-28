// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.ObjectColumns;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddObjectColumnConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjectColumn>()
                .ToTable("ObjectColumns", "Configuration");

            modelBuilder.Entity<ObjectColumn>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.Id)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.SpecificationObjectId)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.SupplierColumnName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.OurColumnName)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.ColumnDescription)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.PopulatedBy)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.SqlDataType)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.FhirDataType)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.SupplierDateFormat)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsWatermark)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsSequencing)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsBusinessKey)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsUniqueRecordKey)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsVersionHashElement)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsSenderCode)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsAuthorCode)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsDeleteFlag)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsPersonConfidentialData)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.PersonConfidentialDataType)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.MaskingMethod)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsSensitiveRecordMarker)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.CodeSystem)
                .HasMaxLength(255)
                .IsRequired(true);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.PartitionColumnLevel)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsForeignKey)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.ForeignKeyTableName)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.ForeignKeyColumnName)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsCaseSensitive)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.DeleteCondition)
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsNumeric)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.IsPostcode)
                .HasDefaultValue(false)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .Property(objectColumn => objectColumn.UpdatedDate)
                .IsRequired();

            modelBuilder.Entity<ObjectColumn>()
                .HasOne(columnDefinition => columnDefinition.SpecificationObject)
                .WithMany(schemaDefinition => schemaDefinition.ObjectColumns)
                .HasForeignKey(columnDefinition => columnDefinition.SpecificationObjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}