// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Services.Foundations.DataTypes;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataTypeService dataTypeService;

        public DataTypeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.dataTypeService = new DataTypeService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataType CreateRandomDataType() =>
            CreateDataTypeFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static DataType CreateRandomDataType(DateTimeOffset dateTimeOffset) =>
            CreateDataTypeFiller(dateTimeOffset).Create();

        private static Filler<DataType> CreateDataTypeFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataType>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(dataType => dataType.CreatedBy).Use(user)
                .OnProperty(dataType => dataType.UpdatedBy).Use(user)
                .OnProperty(dataType => dataType.ColumnDefinitions).IgnoreIt();

            return filler;
        }
    }
}