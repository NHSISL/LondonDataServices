// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        private readonly Mock<IDataSetSpecificationService> dataSetSpecificationServiceMock =
            new Mock<IDataSetSpecificationService>();

        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IDataSetSpecificationProcessingService dataSetSpecificationProcessingService;

        public DataSetSpecificationProcessingServiceTests()
        {
            dataSetSpecificationProcessingService = new DataSetSpecificationProcessingService(
                dataSetSpecificationService: dataSetSpecificationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.", innerException),

                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification validation errors occurred, please try again.", innerException),

                new DataSetSpecificationServiceException(
                    message : "DataSetSpecification service error occurred, contact support.", innerException)
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSetSpecification CreateRandomDataSetSpecification() =>
            CreateDataSetSpecificationFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications()
        {
            return CreateDataSetSpecificationFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(length: 255).ToString();
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }
    }
}
