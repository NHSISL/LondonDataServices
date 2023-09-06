// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
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
                .OnProperty(DataSet => DataSet.Id).Use(inputDataSet.Id)
                .OnProperty(DataSet => DataSet.CreatedBy).Use(inputDataSet.CreatedBy)
                .OnProperty(DataSet => DataSet.CreatedDate).Use(inputDataSet.CreatedDate)
                .OnProperty(DataSet => DataSet.ActiveFrom).Use(inputDataSet.ActiveFrom)
                .OnProperty(DataSet => DataSet.ActiveTo).Use(inputDataSet.ActiveTo)
                .OnProperty(DataSet => DataSet.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
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