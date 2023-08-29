// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.ObjectColumns;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddObjectColumnConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjectColumn>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<ObjectColumn>()
                .HasOne(columnDefinition => columnDefinition.DataSetObject)
                .WithMany(schemaDefinition => schemaDefinition.DataSetObjects)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetObjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}