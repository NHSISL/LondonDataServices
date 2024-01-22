// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSupplierSeedData(ModelBuilder modelBuilder)
        {
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Id = Guid.Parse("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                    Name = "EMIS",
                    FriendlyName = "EMIS",
                    Description = "Emis Supplier",
                    CreatedBy = "System",
                    CreatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0),
                    UpdatedBy = "System",
                    UpdatedDate = new DateTime(year: 2023, month: 1, day: 1,hour: 0,minute: 0, second: 0)
                }
            };

            modelBuilder.Entity<Supplier>().HasData(suppliers);
        }
    }
}