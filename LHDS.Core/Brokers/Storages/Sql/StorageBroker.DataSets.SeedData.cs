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
                    Id = Guid.Parse("6A62313A-7442-462E-B6E8-DEC541DDD0BA"),
                    SupplierId = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                    DataSetName = "PrimaryCareEMIS",
                    DataSetAliases = "EMIS",
                    DataSetAuthor = "EMIS",
                    SpecifiedBy = "EMIS",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    UpdatedBy = "System",
                    UpdatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                }
            };

            modelBuilder.Entity<DataSet>().HasData(dataSets);
        }
    }
}
