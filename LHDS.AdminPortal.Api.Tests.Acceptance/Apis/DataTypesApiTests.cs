// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataType UpdateDataTypeWithRandomValues(DataType inputDataType)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<DataType>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(DataType => DataType.Id).Use(inputDataType.Id)
                .OnProperty(DataType => DataType.CreatedBy).Use(inputDataType.CreatedBy)
                .OnProperty(DataType => DataType.CreatedDate).Use(inputDataType.CreatedDate)
                .OnProperty(DataType => DataType.UpdatedDate).Use(now);

            return filler.Create();
        }

        private static IQueryable<DataType> CreateRandomDataTypes()
        {
            return CreateDataTypeFiller()
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DataType CreateRandomDataType() =>
           CreateDataTypeFiller().Create();

        private static Filler<DataType> CreateDataTypeFiller()
        {
            string user = GetRandomString(255).ToString();
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