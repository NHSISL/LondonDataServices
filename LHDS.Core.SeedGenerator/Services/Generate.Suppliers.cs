// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Suppliers;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate
    {
        private static List<Supplier> CreateRandomSuppliers(int count)
        {
            return CreateSupplierFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: count)
                    .ToList();
        }

        private static Filler<Supplier> CreateSupplierFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.CreatedBy).Use(user)
                .OnProperty(supplier => supplier.UpdatedBy).Use(user);

            return filler;
        }
    }
}
