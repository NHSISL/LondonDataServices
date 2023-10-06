// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SpecificationObjects
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SpecificationObjectsApiTests
    {
        private readonly ApiBroker apiBroker;

        public SpecificationObjectsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<SpecificationObject> CreateRandomSpecificationObjects(Guid dataSetSpecificationId)
        {
            return CreateSpecificationObjectFiller(dataSetSpecificationId)
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SpecificationObject UpdateSpecificationObjectWithRandomValues(
            SpecificationObject inputSpecificationObject)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SpecificationObject>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(SpecificationObject => SpecificationObject.Id).Use(inputSpecificationObject.Id)

                .OnProperty(SpecificationObject =>
                    SpecificationObject.DataSetSpecificationId).Use(inputSpecificationObject.DataSetSpecificationId)

                .OnProperty(SpecificationObject =>
                    SpecificationObject.CreatedBy).Use(inputSpecificationObject.CreatedBy)

                .OnProperty(SpecificationObject =>
                    SpecificationObject.CreatedDate).Use(inputSpecificationObject.CreatedDate)

                .OnProperty(DataSet => DataSet.UpdatedDate).Use(now);

            return filler.Create();
        }

        private static SpecificationObject CreateRandomSpecificationObject(Guid dataSetSpecificationId) =>
            CreateSpecificationObjectFiller(dataSetSpecificationId).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(Guid dataSetSpecificationId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SpecificationObject>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)

                .OnProperty(SpecificationObject => 
                    SpecificationObject.DataSetSpecificationId).Use(dataSetSpecificationId)

                .OnProperty(SpecificationObject => SpecificationObject.CreatedBy).Use(user)
                .OnProperty(SpecificationObject => SpecificationObject.UpdatedBy).Use(user);

            return filler;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(Guid dataSetId) =>
            CreateDataSetSpecificationFiller(dataSetId).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(Guid dataSetId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.DataSetId).Use(dataSetId)

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

        private static IQueryable<DataSet> CreateRandomDataSets(Guid supplierId)
        {
            return CreateDataSetFiller(supplierId)
                .Create(count: GetRandomNumber())
                    .AsQueryable();
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