// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LandingsApiTests
    {
        private readonly ApiBroker apiBroker;

        public LandingsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(supplierId, fileName, encryptedFilePath, decryptedFilePath);

            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath) =>
            CreateRandomIngestionTrackingFiller(supplierId, fileName,  encryptedFilePath, decryptedFilePath).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(
            Guid supplierId,
            string fileName,
            string encryptedFilePath,
            string decryptedFilePath)
        {
            string user = GetRandomString(255);
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use($"{fileName}")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.EncryptedFileName).Use($"/{encryptedFilePath}{fileName}")

                .OnProperty(ingestionTracking =>
                    ingestionTracking.DecryptedFileName).Use($"/{decryptedFilePath}{fileName}")

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
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }
    }
}