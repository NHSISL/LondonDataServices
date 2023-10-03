// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSpecificationSeedData(ModelBuilder modelBuilder)
        {
            List<DataSetSpecification> dataSetSpecifications = new List<DataSetSpecification>
            {
                new DataSetSpecification
                {
                    Id = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    DataSetId = Guid.Parse("6A62313A-7442-462E-B6E8-DEC541DDD0BA"),
                    SupplierSpecificationVersion = "7",
                    OurSpecificationVersion = "1",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<DataSetSpecification>().HasData(dataSetSpecifications);
        }
    }
}
