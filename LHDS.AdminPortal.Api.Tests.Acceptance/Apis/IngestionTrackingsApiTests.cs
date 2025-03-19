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
using Xunit.Abstractions;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackings
{
    [Collection(nameof(ApiTestCollection))]
    public partial class IngestionTrackingsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly ITestOutputHelper output;

        public IngestionTrackingsApiTests(ApiBroker apiBroker, ITestOutputHelper output)
        {
            this.apiBroker = apiBroker;
            this.output = output;
        }

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IngestionTracking UpdateIngestionTrackingWithRandomValues(IngestionTracking inputIngestionTracking)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.Id).Use(inputIngestionTracking.Id)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(inputIngestionTracking.SupplierId)
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(inputIngestionTracking.FileName)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(inputIngestionTracking.CreatedBy)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(inputIngestionTracking.CreatedDate)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);

            IngestionTracking storageIngestionTracking = 
                await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return storageIngestionTracking;
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

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
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