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
            modelBuilder.Entity<ObjectColumn>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<ObjectColumn>()
                .HasOne(columnDefinition => columnDefinition.DatasetObject)
                .WithMany(schemaDefinition => schemaDefinition.DatasetObjects)
                .HasForeignKey(columnDefinition => columnDefinition.DatasetObjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}