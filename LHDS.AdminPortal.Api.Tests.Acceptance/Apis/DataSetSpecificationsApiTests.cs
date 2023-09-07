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
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DataSetSpecificationsApiTests
    {
        private readonly ApiBroker apiBroker;

        public DataSetSpecificationsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications()
        {
            return CreateDataSetSpecificationFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

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
                .OnProperty(DataSetSpecification => DataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller().Create();

        private static Filler<DataSet> CreateDataSetFiller()
        {
            string user = Guid.NewGuid().ToString();
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