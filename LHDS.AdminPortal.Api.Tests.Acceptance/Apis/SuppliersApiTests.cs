// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
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
            Supplier storageSupplier =  await this.apiBroker.PostSupplierAsync(randomSupplier);

            return storageSupplier;
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

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private async ValueTask<List<IngestionTracking>> PostRandomIngestionTrackingsAsync(Guid supplierId)
        {
            int randomNumber = GetRandomNumber();
            var randomIngestionTrackings = new List<IngestionTracking>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomIngestionTrackings.Add(await PostRandomIngestionTrackingAsync(supplierId));
            }

            return randomIngestionTrackings;
        }

        private static IngestionTracking CreateRandomIngestionTracking(Guid supplierId) =>
            CreateRandomIngestionTrackingFiller(supplierId).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }
    }
}