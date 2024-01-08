// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Workflows
{
    [Collection(nameof(ApiTestCollection))]
    public partial class EmisLandingsApiTests
    {
        private readonly ApiBroker apiBroker;

        public EmisLandingsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min, max).GetValue();

        private static string GetRandomFileName()
        {
            string filename = GetRandomString();

            for (int i = 0; i < 6; i++)
            {
                filename = $"{filename}_{GetRandomString(10)}";
            }

            return filename;
        }

        private static Supplier CreateRandomSupplier(Guid supplierId, DateTimeOffset dateTimeOffset) =>
            CreateSupplierFiller(supplierId, dateTimeOffset).Create();

        private static Filler<Supplier> CreateSupplierFiller(Guid supplierId, DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.Id).Use(supplierId)
                .OnProperty(supplier => supplier.CreatedBy).Use(user)
                .OnProperty(supplier => supplier.UpdatedBy).Use(user);

            return filler;
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
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(2))
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(DataSet dataSet) =>
            CreateDataSetSpecificationFiller(dataSet).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DataSet dataSet)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSet.Id)
                .OnProperty(dataSetSpecification => dataSetSpecification.IsActive).Use(true)
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveTo).Use(now.AddDays(2))

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
