// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.Files.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using LHDS.ConfigImportExportTool.Services.Foundations.Files;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        private readonly Mock<IFileService> fileServiceMock;
        private readonly Mock<ICsvHelperService> csvHelperServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IReadSchemaOrchestrationService readSchemaOrchestrationService;

        public ReadSchemaOrchestrationServiceTests()
        {
            this.fileServiceMock = new Mock<IFileService>();
            this.csvHelperServiceMock = new Mock<ICsvHelperService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.readSchemaOrchestrationService = new ReadSchemaOrchestrationService(
                fileService: this.fileServiceMock.Object,
                csvHelperService: this.csvHelperServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<SpecificationObject> CreateRandomSpecificationObjects(
            List<ObjectColumn> objectColumns)
        {
            return CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SpecificationObject CreateRandomSpecificationObject(List<ObjectColumn> objectColumns) =>
            CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset(), objectColumns).Create();

        private static SpecificationObject CreateRandomSpecificationObject(DateTimeOffset dateTimeOffset, List<ObjectColumn> objectColumns) =>
            CreateSpecificationObjectFiller(dateTimeOffset, objectColumns).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(DateTimeOffset dateTimeOffset, List<ObjectColumn> objectColumns)
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
                .OnProperty(specificationObject => specificationObject.ObjectColumns).Use(objectColumns)
                .OnProperty(specificationObject => specificationObject.DataSetSpecification).IgnoreIt();

            return filler;
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

        private static List<CannonicalSchemaItem> CreateRandomCannonicalSchemaItems()
        {
            return CreateCannonicalSchemaItemFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static CannonicalSchemaItem CreateRandomCannonicalSchemaItem() =>
            CreateCannonicalSchemaItemFiller().Create();

        private static Filler<CannonicalSchemaItem> CreateCannonicalSchemaItemFiller()
        {
            var filler = new Filler<CannonicalSchemaItem>();

            return filler;
        }

        public static TheoryData<Xeption> ReadSchemaOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new FileValidationException(
                    message: "File validation error occured, please contact support.",
                    innerException),

                new FileDependencyValidationException(
                    message: "File dependency validation error occurred, please contact support.",
                    innerException),

                new CsvHelperClientValidationException(innerException),
            };
        }

        public static TheoryData<Xeption> ReadSchemaOrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new FileDependencyException(
                    message: "File dependency error occurred, please contact support.",
                    innerException),

                new FileServiceException(
                    message: "File service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
            };
        }
    }
}