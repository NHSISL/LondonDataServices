// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Orchestrations.SchemaConfigs;
using LHDS.ConfigImportExportTool.Services.Processings.DataSets;
using LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns;
using LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        private readonly Mock<IObjectColumnProcessingService> objectColumnProcessingServiceMock;
        private readonly Mock<ISpecificationObjectProcessingService> specificationObjectProcessingServiceMock;
        private readonly Mock<IDataSetProcessingService> dataSetProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISchemaConfigOrchestrationService schemaConfigOrchestrationService;

        public SchemaConfigOrchestrationServiceTests()
        {
            this.objectColumnProcessingServiceMock = new Mock<IObjectColumnProcessingService>();
            this.specificationObjectProcessingServiceMock = new Mock<ISpecificationObjectProcessingService>();
            this.dataSetProcessingServiceMock = new Mock<IDataSetProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.schemaConfigOrchestrationService = new SchemaConfigOrchestrationService(
                objectColumnService: this.objectColumnProcessingServiceMock.Object,
                specificationObjectService: this.specificationObjectProcessingServiceMock.Object,
                dataSetProcessingService: this.dataSetProcessingServiceMock.Object,
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

        private static List<SchemaConfig> CreateRandomSchemaConfigs()
        {
            return CreateSchemaConfigFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static SchemaConfig CreateRandomSchemaConfig() =>
            CreateSchemaConfigFiller().Create();

        private static Filler<SchemaConfig> CreateSchemaConfigFiller()
        {
            var filler = new Filler<SchemaConfig>();

            return filler;
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