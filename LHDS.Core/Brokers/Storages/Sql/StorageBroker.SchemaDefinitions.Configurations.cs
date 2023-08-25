// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.SchemaDefinitions;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSchemaDefinitionConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DatasetObject>()
                .ToTable(columnDefinition => columnDefinition.IsTemporal());

            modelBuilder.Entity<DatasetObject>()
                .HasOne(schemaDefinition => schemaDefinition.DatasetSpecification)
                .WithMany(dataSet => dataSet.DatasetObjects)
                .HasForeignKey(schemaDefinition => schemaDefinition.DatasetSpecificationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}