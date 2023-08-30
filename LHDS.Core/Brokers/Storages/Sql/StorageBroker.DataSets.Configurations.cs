// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSet>()
                .ToTable(dataSet => dataSet.IsTemporal());

            modelBuilder.Entity<DataSet>()
               .Property(objectColumn => objectColumn.Id)
               .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.DataSetName)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.DataSetAliasses)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.DataSetSupplier)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.DataSetAuthor)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
               .Property(objectColumn => objectColumn.DataSourceType)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.ActiveFrom)
                .IsRequired(false);

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.ActiveTo)
                .IsRequired(false);

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.UpdatedBy)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<DataSet>()
                .Property(objectColumn => objectColumn.UpdatedDate)
                .IsRequired();
        }
    }
}