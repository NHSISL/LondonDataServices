// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataTypeConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataType>()
                .ToTable("DataTypes", "Configurations");

            modelBuilder.Entity<DataType>()
                .ToTable(dataType => dataType.IsTemporal());

            modelBuilder.Entity<DataType>()
                .Property(dataType => dataType.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<DataType>()
                .Property(dataType => dataType.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataType>()
                .Property(dataType => dataType.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataType>()
                .Property(dataType => dataType.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataType>()
                .Property(dataType => dataType.UpdatedDate)
                .IsRequired();
        }
    }
}