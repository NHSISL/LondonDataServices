// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports;
using LHDS.ConfigImportExportTool.Services.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Coordinations.ImportExports
{
    public partial class ImportExportCoordinationServiceTests
    {
        private readonly Mock<IReadSchemaOrchestrationService> readSchemaOrchestrationServiceMock;
        private readonly Mock<ISchemaConfigOrchestrationService> schemaConfigOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IImportExportCoordinationService importExportCoordinationService;

        public ImportExportCoordinationServiceTests()
        {
            this.readSchemaOrchestrationServiceMock = new Mock<IReadSchemaOrchestrationService>();
            this.schemaConfigOrchestrationServiceMock = new Mock<ISchemaConfigOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.importExportCoordinationService = new ImportExportCoordinationService(
                readSchemaOrchestrationService: this.readSchemaOrchestrationServiceMock.Object,
                schemaConfigOrchestrationService: this.schemaConfigOrchestrationServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static List<ObjectColumn> CreateRandomObjectColumns(Guid specificationId)
        {
            return CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset(), specificationId)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<ObjectColumn> CreateObjectColumnFiller(
            DateTimeOffset dateTimeOffset,
            Guid specificationId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(objectColumn => objectColumn.SpecificationObjectId).Use(specificationId)
                .OnProperty(objectColumn => objectColumn.CreatedBy).Use(user)
                .OnProperty(objectColumn => objectColumn.UpdatedBy).Use(user);

            return filler;
        }

        private static List<SpecificationObject> CreateRandomSpecificationObjects()
        {
            return CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).ToList();
        }

        private static SpecificationObject CreateRandomSpecificationObject() =>
            CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static SpecificationObject CreateRandomSpecificationObject(DateTimeOffset dateTimeOffset) =>
            CreateSpecificationObjectFiller(dateTimeOffset).Create();

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(DateTimeOffset dateTimeOffset)
        {
            string user = GetRandomString(255);
            Guid dataSetSpecificationId = Guid.NewGuid();
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
                .OnProperty(specificationObject => specificationObject.DataSetSpecificationId).Use(dataSetSpecificationId)
                .OnProperty(specificationObject => specificationObject.DataSetSpecification).IgnoreIt()

                .OnProperty(specificationObject => specificationObject.ObjectColumns)
                    .Use(CreateRandomObjectColumns(dataSetSpecificationId));

            return filler;
        }

        public static TheoryData<Xeption> ImportExportCoordinationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ReadSchemaValidationOrchestrationException(
                    message: "Read schema orchestration validation error occured, please contact support.",
                    innerException),

                new ReadSchemaOrchestrationDependencyValidationException(
                    message: "Read schema orchestration dependency validation error occurred, please contact support.",
                    innerException),

                new SchemaConfigValidationOrchestrationException(
                    message: "Schema config orchestration validation error occured, please contact support.",
                    innerException),

                new SchemaConfigOrchestrationDependencyValidationException(
                    message:
                        "Schema config orchestration dependency validation error occurred, please contact support.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> ImportExportCoordinationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ReadSchemaOrchestrationDependencyException(
                    message: "Read schema orchestration dependency error occurred, please contact support.",
                    innerException),

                new ReadSchemaOrchestrationServiceException(
                    message: "Read schema orchestration service error occurred, please contact support.",
                    innerException),

                new SchemaConfigOrchestrationDependencyException(
                    message: "Schema config orchestration dependency error occurred, please contact support.",
                    innerException),

                new SchemaConfigOrchestrationServiceException(
                    message: "Schema config orchestration service error occurred, please contact support.",
                    innerException),
            };
        }
    }
}