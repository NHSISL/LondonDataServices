// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.ColumnDefinitions;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddColumnDefinitionConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColumnDefinition>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<ColumnDefinition>()
                .HasOne(columnDefinition => columnDefinition.DataType)
                .WithMany(dataType => dataType.ColumnDefinitions)
                .HasForeignKey(columnDefinition => columnDefinition.DataTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ColumnDefinition>()
                .HasOne(columnDefinition => columnDefinition.SchemaDefinition)
                .WithMany(schemaDefinition => schemaDefinition.ColumnDefinitions)
                .HasForeignKey(columnDefinition => columnDefinition.SchemaDefinitionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}