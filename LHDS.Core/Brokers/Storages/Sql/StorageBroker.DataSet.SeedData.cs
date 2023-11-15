// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSeedData(ModelBuilder modelBuilder)
        {
            List<DataSet> dataSets = new List<DataSet>
            {
                new DataSet
                {
                    Id = Guid.Parse("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                    SupplierId = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                    DataSetName = "PrimaryCareEMISDEV",
                    DataSetAliases = "PrimaryCareEMISDEV",
                    DataSetAuthor = "EMISDEV",
                    SpecifiedBy = "EMIS",
                    IsNationallySpecified = false,
                    CollectedBy = "EMIS",
                    IsNationallyCollected = false,
                    DataSourceType = "PrimaryCareEMISDEV",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    UpdatedBy = "System",
                    UpdatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    ActiveFrom = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    ActiveTo = new DateTime(year: 2123, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    IsActive = true,
                }
            };

            modelBuilder.Entity<DataSet>().HasData(dataSets);
        }
    }
}