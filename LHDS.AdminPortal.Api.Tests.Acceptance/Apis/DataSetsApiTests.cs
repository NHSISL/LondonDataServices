// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSets
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DataSetsApiTests
    {
        private readonly ApiBroker apiBroker;

        public DataSetsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSet UpdateDataSetWithRandomValues(DataSet inputDataSet)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(DataSet => DataSet.Id).Use(inputDataSet.Id)
                .OnProperty(DataSet => DataSet.SupplierId).Use(inputDataSet.SupplierId)
                .OnProperty(DataSet => DataSet.CreatedBy).Use(inputDataSet.CreatedBy)
                .OnProperty(DataSet => DataSet.CreatedDate).Use(inputDataSet.CreatedDate)
                .OnProperty(DataSet => DataSet.ActiveFrom).Use(inputDataSet.ActiveFrom)
                .OnProperty(DataSet => DataSet.ActiveTo).Use(inputDataSet.ActiveTo)
                .OnProperty(DataSet => DataSet.UpdatedDate).Use(now);

            return filler.Create();
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