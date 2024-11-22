// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
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
        private readonly ICompareLogic compareLogic;
        private readonly IReadSchemaOrchestrationService readSchemaOrchestrationService;

        public ReadSchemaOrchestrationServiceTests()
        {
            this.fileServiceMock = new Mock<IFileService>();
            this.csvHelperServiceMock = new Mock<ICsvHelperService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.readSchemaOrchestrationService = new ReadSchemaOrchestrationService(
                fileService: this.fileServiceMock.Object,
                csvHelperService: this.csvHelperServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<List<CannonicalSchemaItem>, bool>> SameCannonicalSchemaItemListAs(List<CannonicalSchemaItem> expectedCannonicalSchemaItems)
        {
            return actualCannonicalSchemaItems =>
                this.compareLogic.Compare(expectedCannonicalSchemaItems, actualCannonicalSchemaItems)
                    .AreEqual;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<SpecificationObject> CreateRandomSpecificationObjects(
            List<ObjectColumn> objectColumns, string tableName)
        {
            return CreateSpecificationObjectFiller(objectColumns, tableName)
                .Create(count: GetRandomNumber()).ToList();
        }

        private static SpecificationObject CreateRandomSpecificationObject(
            List<ObjectColumn> objectColumns,
            string tableName) =>
            CreateSpecificationObjectFiller(objectColumns, tableName).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(
            List<ObjectColumn> objectColumns,
            string tableName)
        {
            string user = GetRandomString(255);
            var filler = new Filler<SpecificationObject>();

            filler.Setup()
                .OnType<DateTimeOffset>().IgnoreIt()
                .OnType<DateTimeOffset?>().IgnoreIt()
                .OnType<Guid>().IgnoreIt()
                .OnProperty(specificationObject => specificationObject.SupplierObjectName).Use(tableName)
                .OnProperty(specificationObject => specificationObject.ObjectColumns).Use(objectColumns)
                .OnProperty(specificationObject => specificationObject.DataSetSpecificationId).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.OurObjectName).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.ObjectDescription).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.InterchangeProtocol).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.IsPushedToUs).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.IsPulledByUs).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.DeletionHandling).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.IsSubmissionHeaderObject).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.IsTransactionLog).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.CreatedBy).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.UpdatedBy).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.DataSetSpecification).IgnoreIt();

            return filler;
        }

        private static List<ObjectColumn> CreateRandomObjectColumns()
        {
            return CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).ToList();
        }

        private static ObjectColumn CreateRandomObjectColumn() =>
            CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ObjectColumn CreateRandomObjectColumn(DateTimeOffset dateTimeOffset) =>
            CreateObjectColumnFiller(dateTimeOffset).Create();

        private static Filler<ObjectColumn> CreateObjectColumnFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().IgnoreIt()
                .OnType<DateTimeOffset?>().IgnoreIt()
                .OnType<string>().IgnoreIt()
                .OnType<bool>().IgnoreIt()
                .OnType<int?>().IgnoreIt()
                .OnType<int>().IgnoreIt()
                .OnType<Guid>().IgnoreIt()
                .OnProperty(objectColumn => objectColumn.SupplierColumnName).Use(GetRandomString())
                .OnProperty(objectColumn => objectColumn.SqlDataType).Use(GetRandomString())
                .OnProperty(objectColumn => objectColumn.ColumnDescription).Use(GetRandomString())
                .OnProperty(objectColumn => objectColumn.Length).Use(GetRandomNumber())
                .OnProperty(objectColumn => objectColumn.OrdinalPosition).Use(GetRandomNumber())
                .OnProperty(objectColumn => objectColumn.ForeignKeyTableName).Use(GetRandomString())
                .OnProperty(objectColumn => objectColumn.ForeignKeyColumnName).Use(GetRandomString())
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