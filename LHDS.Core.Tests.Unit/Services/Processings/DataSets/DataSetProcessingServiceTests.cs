// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using LHDS.Core.Services.Foundations.DataSets;
using LHDS.Core.Services.Processings.DataSets;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        private readonly Mock<IDataSetService> dataSetServiceMock = new Mock<IDataSetService>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IDataSetProcessingService dataSetProcessingService;

        public DataSetProcessingServiceTests()
        {
            dataSetProcessingService = new DataSetProcessingService(
                dataSetService: dataSetServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
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

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DataSet CreateRandomDataSet() =>
            CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IQueryable<DataSet> CreateRandomDataSets()
        {
            return CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<DataSet> CreateDataSetFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.Supplier).IgnoreIt()
                .OnProperty(dataSet => dataSet.DataSetSpecifications).IgnoreIt();

            return filler;
        }
    }
}