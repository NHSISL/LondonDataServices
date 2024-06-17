// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSets;
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

        public static TheoryData<IQueryable<DataSetSpecification>, Guid> CountValidations()
        {
            Guid randomSupplierId = Guid.NewGuid();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplierId);

            return new TheoryData<IQueryable<DataSetSpecification>, Guid>
            {
                {
                    new List<DataSetSpecification>().AsQueryable(),
                    randomSupplierId
                },

                {
                    CreateRandomDataSetSpecifications(dataSet: randomDataSet, dataSetId: randomDataSet.Id, count: 2),
                    randomSupplierId
                }
            };
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
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

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification validation errors occurred, please try again.", innerException),

                new DataSetSpecificationServiceException(
                    message : "DataSetSpecification service error occurred, please contact support.", innerException)
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
                .Create(count: 1)
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
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SpecificationObjects).IgnoreIt();

            return filler;
        }

        private static DataSet CreateRandomDataSet(Guid supplierId) =>
           CreateDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.Supplier).IgnoreIt()
                .OnProperty(dataSet => dataSet.DataSetSpecifications).IgnoreIt()
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications(
            DataSet dataSet,
            Guid dataSetId,
            int count)
        {
            return CreateDataSetSpecificationFiller(dataSet, dataSetId)
                .Create(count)
                    .AsQueryable();
        }

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DataSet dataSet, Guid dataSetId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.DataSetId).Use(dataSetId)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.DataSet).Use(dataSet)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.IsActive).Use(true)

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SpecificationObjects).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }
    }
}
