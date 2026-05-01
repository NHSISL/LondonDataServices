// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;


namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DataSetSpecificationsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly ITestOutputHelper output;

        public DataSetSpecificationsApiTests(ApiBroker apiBroker, ITestOutputHelper output)
        {
            this.apiBroker = apiBroker;
            this.output = output;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSetSpecification UpdateDataSetSpecificationWithRandomValues(
            DataSetSpecification inputDataSetSpecification)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(() => now)
                .OnType<DateTimeOffset>().Use(() => now)
                .OnProperty(dataSetSpecification => dataSetSpecification.Id).Use(inputDataSetSpecification.Id)

                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy)
                    .Use(inputDataSetSpecification.CreatedBy)

                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedDate)
                    .Use(inputDataSetSpecification.CreatedDate)

                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId)
                    .Use(inputDataSetSpecification.DataSetId)

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById)
                    .Use(inputDataSetSpecification.PresededById)

                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById)
                    .Use(inputDataSetSpecification.SupersededById)

                .OnProperty(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(GetRandomString(10))
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedDate).Use(now);

            var modifiedDataSetSpecification = filler.Create();

            return modifiedDataSetSpecification;
        }

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications(Guid dataSetId)
        {
            return CreateDataSetSpecificationFiller(dataSetId)
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(Guid dataSetId) =>
            CreateDataSetSpecificationFiller(dataSetId).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(Guid dataSetId)
        {
            string user = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(() => now)
                .OnType<DateTimeOffset>().Use(() => now)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSetId)

                .OnProperty(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                    .Use(() => GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                    .Use(() => GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(() => user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedDate).Use(() => now)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(() => user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedDate).Use(() => now);

            return filler;
        }

        private async ValueTask<DataSet> PostRandomDataSetAsync(Guid supplierId)
        {
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            return randomDataSet;
        }

        private static DataSet CreateRandomDataSet(Guid supplierId) =>
            CreateDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

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
