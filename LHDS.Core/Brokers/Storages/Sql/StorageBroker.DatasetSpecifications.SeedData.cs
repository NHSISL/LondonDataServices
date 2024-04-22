// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddDataSetSpecificationsSeedData(ModelBuilder modelBuilder)
        {
            List<DataSetSpecification> dataSets = new List<DataSetSpecification>
            {
                new DataSetSpecification
                {
                    Id = Guid.Parse("e8ebce80-e619-40ca-b45f-9c3ac0328143"),
                    DataSetId = Guid.Parse("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                    SupplierSpecificationVersion = "7.0",
                    OurSpecificationVersion = "1.0",
                    Notes = "This is a test dataset specification",
                    IsMultiAuthorPerBatch = true,
                    EntityChangeSynchronisation = "",
                    DateReleased = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    DateImplemented = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    DateSuperseded = null,
                    IsPublished = true,
                    PresededById = null,
                    SupersededById = null,
                    CreatedBy = "System",
                    CreatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    UpdatedBy = "System",
                    UpdatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    ActiveFrom = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    ActiveTo = new DateTime(year: 2123, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    IsActive = true,
                }
            };

            modelBuilder.Entity<DataSetSpecification>().HasData(dataSets);
        }
    }
}