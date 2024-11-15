// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        private readonly Mock<ISpecificationObjectService> specificationObjectServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISpecificationObjectProcessingService specificationObjectProcessingService;

        public SpecificationObjectProcessingServiceTests()
        {
            specificationObjectServiceMock = new Mock<ISpecificationObjectService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            specificationObjectProcessingService = new SpecificationObjectProcessingService(
                specificationObjectService: specificationObjectServiceMock.Object,
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
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.", innerException),

                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.", innerException)
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject validation errors occurred, please try again.", innerException),

                new SpecificationObjectServiceException(
                    message : "SpecificationObject service error occurred, please contact support.", innerException)
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