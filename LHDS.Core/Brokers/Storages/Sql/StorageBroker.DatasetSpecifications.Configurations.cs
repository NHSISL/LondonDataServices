// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSpecificationConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSetSpecification>()
                .ToTable(dataSet => dataSet.IsTemporal());

            modelBuilder.Entity<DataSetSpecification>()
                .HasOne(columnDefinition => columnDefinition.DataSet)
                .WithMany(schemaDefinition => schemaDefinition.DataSetSpecifications)
                .HasForeignKey(columnDefinition => columnDefinition.DataSetId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}