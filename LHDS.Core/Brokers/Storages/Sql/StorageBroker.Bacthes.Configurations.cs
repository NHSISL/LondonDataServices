// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Batches;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddBatchConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>()
                .ToTable("Batch", "LDS");

            modelBuilder.Entity<Batch>()
                .ToTable(dataSet => dataSet.IsTemporal());

            modelBuilder.Entity<Batch>()
               .Property(dataSet => dataSet.Id)
               .IsRequired();

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.BusinessKey)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.SourceSystem)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.Status)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.ErrorMessage)
                .IsRequired(false);

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.StartDateTime)
                .IsRequired();

            modelBuilder.Entity<Batch>()
                .Property(dataSet => dataSet.EndDateTime)
                .IsRequired(false);
        }
    }
}