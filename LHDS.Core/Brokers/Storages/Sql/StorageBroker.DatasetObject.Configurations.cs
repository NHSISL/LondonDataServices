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
                .HasOne(columnDefinition => columnDefinition.DataSetSpecification)
                .WithMany(schemaDefinition => schemaDefinition.DataSetObjects)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetSpecificationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}