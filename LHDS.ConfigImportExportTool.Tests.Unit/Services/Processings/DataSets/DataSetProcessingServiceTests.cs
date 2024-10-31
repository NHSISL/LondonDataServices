// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        private readonly Mock<IDataSetService> dataSetServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataSetProcessingService dataSetProcessingService;

        public DataSetProcessingServiceTests()
        {
            this.dataSetServiceMock = new Mock<IDataSetService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.dataSetProcessingService = new DataSetProcessingService(
                dataSetService: this.dataSetServiceMock.Object,
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

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.", innerException),

                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DataSetDependencyException(
                    message: "DataSet validation errors occurred, please try again.", innerException),

                new DataSetServiceException(
                    message : "DataSet service error occurred, please contact support.", innerException)
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

        private static IQueryable<DataSet> CreateRandomDataSets()
        {
            return CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<DataSet> CreateDataSetFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)

                .OnProperty(dataSet => dataSet.DataSetName)
                    .Use(GetRandomString(150))

                .OnProperty(dataSet => dataSet.DataSetAliases)
                    .Use(GetRandomString(250))

                .OnProperty(dataSet => dataSet.DataSetAuthor)
                    .Use(GetRandomString(150))

                .OnProperty(dataSet => dataSet.DataSourceType)
                    .Use(GetRandomString(50))

                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.DataSetName).Use(GetRandomString(150))
                .OnProperty(dataSet => dataSet.DataSetAliases).Use(GetRandomString(250))
                .OnProperty(dataSet => dataSet.DataSetAuthor).Use(GetRandomString(150))
                .OnProperty(dataSet => dataSet.DataSourceType).Use(GetRandomString(50))
                .OnProperty(dataSet => dataSet.DataSetSpecifications).IgnoreIt()
                .OnProperty(dataSet => dataSet.Supplier).IgnoreIt();

            return filler;
        }
    }
}