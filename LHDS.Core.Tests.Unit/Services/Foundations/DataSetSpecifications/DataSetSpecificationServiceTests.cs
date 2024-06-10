// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataSetSpecificationService dataSetSpecificationService;

        public DataSetSpecificationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.dataSetSpecificationService = new DataSetSpecificationService(
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

        public static TheoryData<int> MinutesBeforeOrAfter()
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
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSetSpecification CreateRandomModifyDataSetSpecification(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(dateTimeOffset);

            randomDataSetSpecification.CreatedDate =
                randomDataSetSpecification.CreatedDate.AddDays(randomDaysInPast);

            return randomDataSetSpecification;
        }

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications()
        {
            return CreateDataSetSpecificationFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DataSetSpecification CreateRandomDataSetSpecification() =>
            CreateDataSetSpecificationFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static DataSetSpecification CreateRandomDataSetSpecification(DateTimeOffset dateTimeOffset) =>
            CreateDataSetSpecificationFiller(dateTimeOffset).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)

                .OnProperty(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SpecificationObjects).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededBy).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededBy).IgnoreIt();

            return filler;
        }
    }
}