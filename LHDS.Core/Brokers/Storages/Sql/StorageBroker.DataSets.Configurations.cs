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
            modelBuilder.Entity<DatasetSpecification>()
                .ToTable(dataSet => dataSet.IsTemporal());
        }
    }
}