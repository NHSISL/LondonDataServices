// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Suppliers
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SuppliersApiTests
    {
        private readonly ApiBroker apiBroker;

        public SuppliersApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Supplier UpdateSupplierWithRandomValues(Supplier inputSupplier)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnProperty(supplier => supplier.Id).Use(inputSupplier.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(supplier => supplier.CreatedDate).Use(inputSupplier.CreatedDate)
                .OnProperty(supplier => supplier.CreatedBy).Use(inputSupplier.CreatedBy)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private async ValueTask<List<Supplier>> PostRandomSuppliersAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSuppliers = new List<Supplier>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSuppliers.Add(await PostRandomSupplierAsync());
            }

            return randomSuppliers;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }
    }
}