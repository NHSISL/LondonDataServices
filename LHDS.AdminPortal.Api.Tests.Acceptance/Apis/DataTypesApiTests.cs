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

        private static DataType CreateRandomDataType(DateTimeOffset dateTimeOffset) =>
           CreateDataTypeFiller(dateTimeOffset).Create();

        private static Filler<DataType> CreateDataTypeFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataType>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(dataType => dataType.CreatedBy).Use(user)
                .OnProperty(dataType => dataType.UpdatedBy).Use(user);

            return filler;
        }
    }
}