// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISpecificationObjectService specificationObjectService;

        public SpecificationObjectServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.specificationObjectService = new SpecificationObjectService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

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

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SpecificationObject CreateRandomModifySpecificationObject(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject(dateTimeOffset);

            randomSpecificationObject.CreatedDate =
                randomSpecificationObject.CreatedDate.AddDays(randomDaysInPast);

            return randomSpecificationObject;
        }

        private static IQueryable<SpecificationObject> CreateRandomSpecificationObjects()
        {
            return CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SpecificationObject CreateRandomSpecificationObject() =>
            CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static SpecificationObject CreateRandomSpecificationObject(DateTimeOffset dateTimeOffset) =>
            CreateSpecificationObjectFiller(dateTimeOffset).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(255);
            var filler = new Filler<SpecificationObject>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(specificationObject => specificationObject.SupplierObjectName).Use(GetRandomString(255))
                .OnProperty(specificationObject => specificationObject.OurObjectName).Use(GetRandomString(255))
                .OnProperty(specificationObject => specificationObject.ObjectDescription).Use(GetRandomString(500))
                .OnProperty(specificationObject => specificationObject.InterchangeProtocol).Use(GetRandomString(255))
                .OnProperty(specificationObject => specificationObject.DeletionHandling).Use(GetRandomString(255))
                .OnProperty(specificationObject => specificationObject.CreatedBy).Use(user)
                .OnProperty(specificationObject => specificationObject.UpdatedBy).Use(user)
                .OnProperty(specificationObject => specificationObject.ObjectColumns).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.DataSetSpecification).IgnoreIt();

            return filler;
        }
    }
}