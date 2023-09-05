// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataTypes
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DataTypesApiTests
    {
        private readonly ApiBroker apiBroker;

        public DataTypesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataType CreateRandomDataType() =>
           CreateDataTypeFiller().Create();

        private static Filler<DataType> CreateDataTypeFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<DataType>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataType => dataType.CreatedBy).Use(user)
                .OnProperty(dataType => dataType.UpdatedBy).Use(user);

            return filler;
        }
    }
}