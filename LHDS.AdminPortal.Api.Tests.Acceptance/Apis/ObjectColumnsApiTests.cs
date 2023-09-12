// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.ObjectColumns;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
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

        private async ValueTask<SpecificationObject> PostRandomSpecificationObject()
        {
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

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
            var result = await Task.WhenAll(
                Enumerable.Range(start: 1, count: GetRandomNumber())
                    .Select(async _ => await CreateRandomObjectColumnAsync()));

            return result.ToList();
        }

        private async ValueTask CleanupTask(ObjectColumn objectColumn)
        {
            SpecificationObject retrievedSpecificationObject =
                await this.apiBroker.GetSpecificationObjectByIdAsync(objectColumn.SpecificationObjectId);

            DataSetSpecification retrievedDataSetSpecification =
                await this.apiBroker.GetDataSetSpecificationByIdAsync(
                    retrievedSpecificationObject.DataSetSpecificationId);

            await this.apiBroker.DeleteObjectColumnByIdAsync(objectColumn.Id);
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(objectColumn.SpecificationObjectId);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(retrievedDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(retrievedDataSetSpecification.DataSetId);
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

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller().Create();

        private static Filler<DataSet> CreateDataSetFiller()
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }
    }
}