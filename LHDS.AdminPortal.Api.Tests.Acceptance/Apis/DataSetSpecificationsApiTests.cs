// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using Tynamix.ObjectFiller;
using Xunit;
using Xunit.Abstractions;

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

        private static DataSetSpecification CreateRandomDataSetSpecification() =>
            CreateDataSetSpecificationFiller().Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller()
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(DataSetSpecification => 
                    DataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(DataSetSpecification => 
                    DataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(DataSetSpecification => DataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(DataSetSpecification => DataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(DataSetSpecification => DataSetSpecification.CreatedBy).Use(user)
                .OnProperty(DataSetSpecification => DataSetSpecification.CreatedBy).Use(user)
                .OnProperty(DataSetSpecification => DataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }

        private static IQueryable<DataSet> CreateRandomDataSets()
        {
            return CreateDataSetFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
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
                .OnProperty(DataSet => DataSet.CreatedBy).Use(user)
                .OnProperty(DataSet => DataSet.UpdatedBy).Use(user)
                .OnProperty(DataSet => DataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }
    }
}