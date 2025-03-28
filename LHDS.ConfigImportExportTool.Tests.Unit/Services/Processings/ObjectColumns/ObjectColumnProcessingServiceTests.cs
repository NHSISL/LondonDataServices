// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.ConfigImportExportTool.Services.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        private readonly Mock<IObjectColumnService> ObjectColumnServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IObjectColumnProcessingService ObjectColumnProcessingService;

        public ObjectColumnProcessingServiceTests()
        {
            ObjectColumnServiceMock = new Mock<IObjectColumnService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            ObjectColumnProcessingService = new ObjectColumnProcessingService(
                ObjectColumnService: ObjectColumnServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
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

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.", innerException),

                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ObjectColumnDependencyException(
                    message: "ObjectColumn validation errors occurred, please try again.", innerException),

                new ObjectColumnServiceException(
                    message : "ObjectColumn service error occurred, please contact support.", innerException)
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

        private static ObjectColumn CreateRandomModifyObjectColumn(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(dateTimeOffset);

            randomObjectColumn.CreatedDate =
                randomObjectColumn.CreatedDate.AddDays(randomDaysInPast);

            return randomObjectColumn;
        }

        private static IQueryable<ObjectColumn> CreateRandomObjectColumns()
        {
            return CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static ObjectColumn CreateRandomObjectColumn() =>
            CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ObjectColumn CreateRandomObjectColumn(DateTimeOffset dateTimeOffset) =>
            CreateObjectColumnFiller(dateTimeOffset).Create();

        private static Filler<ObjectColumn> CreateObjectColumnFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(objectColumn => objectColumn.CreatedBy).Use(user)
                .OnProperty(objectColumn => objectColumn.UpdatedBy).Use(user)
                .OnProperty(objectColumn => objectColumn.SpecificationObject).IgnoreIt();

            return filler;
        }
    }
}