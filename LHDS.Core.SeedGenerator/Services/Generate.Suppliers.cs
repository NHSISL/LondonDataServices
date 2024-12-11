// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.Suppliers;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate
    {
        private static List<Supplier> CreateRandomSuppliers(int count)
        {
            return CreateSupplierFiller(dateTimeOffset: DateTimeOffset.UtcNow)
                .Create(count: count)
                    .ToList();
        }

        private static Filler<Supplier> CreateSupplierFiller(DateTimeOffset dateTimeOffset)
        {
            Guid id = Guid.NewGuid();
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.Id).Use(id)
                .OnProperty(supplier => supplier.Name).Use(GetRandomString(maxCharacters: 450))
                .OnProperty(supplier => supplier.FriendlyName).Use(GetRandomString(maxCharacters: 450))
                .OnProperty(supplier => supplier.CreatedBy).Use(user)
                .OnProperty(supplier => supplier.UpdatedBy).Use(user);

            return filler;
        }
    }
}
