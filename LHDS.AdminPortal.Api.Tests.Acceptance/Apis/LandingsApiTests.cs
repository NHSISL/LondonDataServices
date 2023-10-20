// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Models.Orchestrations.Downloads;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Landings
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LandingsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly string encryptedFolder;
        private readonly string decryptedFolder;
        private readonly Guid supplierId;

        public LandingsApiTests(
            ApiBroker apiBroker)
        {
            this.apiBroker = apiBroker;
            this.supplierId = this.apiBroker.landingConfiguration.LandingSupplierId;
            this.encryptedFolder = this.apiBroker.landingConfiguration.EncryptedFolder;
            this.decryptedFolder = this.apiBroker.landingConfiguration.DecryptedFolder;
        }

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
            CreateRandomIngestionTrackingFiller(supplierId, fileName, encryptedFilePath, decryptedFilePath).Create();

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

        private async ValueTask<Supplier> PostLandingSupplierAsync(Guid supplierId)
        {
            Supplier landingsSupplier = CreateLandingSupplier(supplierId);
            await this.apiBroker.PostSupplierAsync(landingsSupplier);

            return landingsSupplier;
        }

        private static Supplier CreateLandingSupplier(Guid supplierId) =>
            CreateLandingSupplierFiller(supplierId).Create();

        private static Filler<Supplier> CreateLandingSupplierFiller(Guid supplierId)
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(supplier => supplier.Id).Use(supplierId)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }

        private async ValueTask<DataSet> PostRandomActiveDataSetAsync(Guid supplierId)
        {
            DataSet randomDataSet = CreateRandomActiveDataSet(supplierId);
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            return randomDataSet;
        }

        private static DataSet CreateRandomActiveDataSet(Guid supplierId) =>
            CreateActiveDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateActiveDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private async ValueTask<DataSetSpecification> PostRandomActiveDataSetSpecificationAsync(Guid dataSetId)
        {
            DataSetSpecification randomDataSetSpecification = CreateRandomActiveDataSetSpecification(dataSetId);
            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            return randomDataSetSpecification;
        }

        private static DataSetSpecification CreateRandomActiveDataSetSpecification(Guid dataSetId) =>
            CreateActiveDataSetSpecificationFiller(dataSetId).Create();

        private static Filler<DataSetSpecification> CreateActiveDataSetSpecificationFiller(Guid dataSetId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSetId)
                .OnProperty(dataSetSpecification => dataSetSpecification.IsActive).Use(true)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }
    }
}