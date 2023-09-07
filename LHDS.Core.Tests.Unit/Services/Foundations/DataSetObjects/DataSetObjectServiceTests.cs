// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Services.Foundations.DataSetObjects;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataSetObjectService dataSetObjectService;

        public DataSetObjectServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.dataSetObjectService = new DataSetObjectService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

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

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SpecificationObject CreateRandomModifyDataSetObject(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject(dateTimeOffset);

            randomDataSetObject.CreatedDate =
                randomDataSetObject.CreatedDate.AddDays(randomDaysInPast);

            return randomDataSetObject;
        }

        private static IQueryable<SpecificationObject> CreateRandomDataSetObjects()
        {
            return CreateDataSetObjectFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SpecificationObject CreateRandomDataSetObject() =>
            CreateDataSetObjectFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static SpecificationObject CreateRandomDataSetObject(DateTimeOffset dateTimeOffset) =>
            CreateDataSetObjectFiller(dateTimeOffset).Create();

        private static Filler<SpecificationObject> CreateDataSetObjectFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(255);
            var filler = new Filler<SpecificationObject>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(dataSetObject => dataSetObject.SupplierObjectName).Use(GetRandomString(255))
                .OnProperty(dataSetObject => dataSetObject.OurObjectName).Use(GetRandomString(255))
                .OnProperty(dataSetObject => dataSetObject.ObjectDescription).Use(GetRandomString(500))
                .OnProperty(dataSetObject => dataSetObject.InterchangeProtocol).Use(GetRandomString(255))
                .OnProperty(dataSetObject => dataSetObject.DeletionHandling).Use(GetRandomString(255))
                .OnProperty(dataSetObject => dataSetObject.CreatedBy).Use(user)
                .OnProperty(dataSetObject => dataSetObject.UpdatedBy).Use(user)
                .OnProperty(dataSetObject => dataSetObject.DataSetObjects).IgnoreIt()
                .OnProperty(dataSetObject => dataSetObject.DataSetSpecification).IgnoreIt();

            return filler;
        }
    }
}