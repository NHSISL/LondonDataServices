// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.ObjectColumns
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ObjectColumnsApiTests
    {
        private readonly ApiBroker apiBroker;

        public ObjectColumnsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ObjectColumn UpdateObjectColumnWithRandomValues(
            ObjectColumn inputObjectColumn)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(ObjectColumn => ObjectColumn.Id).Use(inputObjectColumn.Id)

                .OnProperty(ObjectColumn =>
                    ObjectColumn.SpecificationObjectId).Use(inputObjectColumn.SpecificationObjectId)

                .OnProperty(ObjectColumn =>
                    ObjectColumn.CreatedBy).Use(inputObjectColumn.CreatedBy)

                .OnProperty(ObjectColumn =>
                    ObjectColumn.CreatedDate).Use(inputObjectColumn.CreatedDate)

                .OnProperty(DataSet => DataSet.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SpecificationObject> PostRandomSpecificationObject()
        {
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            await this.apiBroker.PostSpecificationObjectAsync(randomSpecificationObject);

            return randomSpecificationObject;
        }

        private async ValueTask<List<ObjectColumn>> CreateRandomObjectColumns()
        {
            List<ObjectColumn> objectColumns = new List<ObjectColumn>();
            int count = GetRandomNumber();

            for (int i = 0; i < count; i++)
            {
                ObjectColumn column = await CreateRandomObjectColumnAsync();
                objectColumns.Add(column);
            }

            return objectColumns;
        }

        private async ValueTask CleanupTask(ObjectColumn objectColumn, bool isObjectColumnDeleted = false)
        {
            SpecificationObject retrievedSpecificationObject =
                await this.apiBroker.GetSpecificationObjectByIdAsync(objectColumn.SpecificationObjectId);

            DataSetSpecification retrievedDataSetSpecification =
                await this.apiBroker.GetDataSetSpecificationByIdAsync(
                    retrievedSpecificationObject.DataSetSpecificationId);

            DataSet retrievedDataSet =
                await this.apiBroker.GetDataSetByIdAsync(
                    retrievedDataSetSpecification.DataSetId);

            if (!isObjectColumnDeleted)
            {
                await this.apiBroker.DeleteObjectColumnByIdAsync(objectColumn.Id);
            }

            await this.apiBroker.DeleteSpecificationObjectByIdAsync(objectColumn.SpecificationObjectId);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(retrievedDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(retrievedDataSetSpecification.DataSetId);
            await this.apiBroker.DeleteSupplierByIdAsync(retrievedDataSet.SupplierId);
        }

        private async ValueTask<ObjectColumn> CreateRandomObjectColumnAsync()
        {
            SpecificationObject randomSpecificationObject = await PostRandomSpecificationObject();

            return CreateObjectColumnFiller(randomSpecificationObject.Id).Create();
        }

        private static Filler<ObjectColumn> CreateObjectColumnFiller(Guid specificationObjectId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ObjectColumn>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)

                .OnProperty(ObjectColumn =>
                    ObjectColumn.SpecificationObjectId).Use(specificationObjectId)

                .OnProperty(ObjectColumn => ObjectColumn.CreatedBy).Use(user)
                .OnProperty(ObjectColumn => ObjectColumn.UpdatedBy).Use(user);

            return filler;
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