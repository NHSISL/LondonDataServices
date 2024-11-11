// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
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
        private readonly Mock<ISpecificationObjectProcessingService> specificationObjectProcessingServiceMock;
        private readonly Mock<IObjectColumnProcessingService> objectColumnProcessingServiceMock;
        private readonly Mock<IDataSetProcessingService> dataSetProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISchemaConfigOrchestrationService schemaConfigOrchestrationService;

        public SchemaConfigOrchestrationServiceTests()
        {
            this.specificationObjectProcessingServiceMock = new Mock<ISpecificationObjectProcessingService>();
            this.objectColumnProcessingServiceMock = new Mock<IObjectColumnProcessingService>();
            this.dataSetProcessingServiceMock = new Mock<IDataSetProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.schemaConfigOrchestrationService = new SchemaConfigOrchestrationService(
                specificationObjectProcessingService: this.specificationObjectProcessingServiceMock.Object,
                objectColumnProcessingService: this.objectColumnProcessingServiceMock.Object,
                dataSetProcessingService: this.dataSetProcessingServiceMock.Object,
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

        private static List<SpecificationObject> CreateRandomSpecificationObjects(Guid dataSetId)
        {
            return CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset(), dataSetId)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(
            DateTimeOffset dateTimeOffset, Guid dataSetId)
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

        public static SchemaConfig CreateRandomSchemaConfig(
            Guid dataSetId, 
            List<SpecificationObject> specificationObjects,
            List<ObjectColumn> objectColumns) =>
                CreateSchemaConfigFiller(dataSetId, specificationObjects, objectColumns).Create();

        private static Filler<SchemaConfig> CreateSchemaConfigFiller(
            Guid dataSetId,
            List<SpecificationObject> specificationObjects,
            List<ObjectColumn> objectColumns)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SchemaConfig>();

            filler.Setup()
                .OnProperty(schemaConfig => schemaConfig.SpecificationObjects).Use(specificationObjects)
                .OnProperty(schemaConfig => schemaConfig.ObjectColumns).Use(objectColumns);

            return filler;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(Guid dataSetId, string version) =>
            CreateDataSetSpecificationFiller(dateTimeOffset: GetRandomDateTimeOffset(), dataSetId, version).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(
            DateTimeOffset dateTimeOffset,
            Guid dataSetId,
            string version)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)

                .OnProperty(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                    .Use(version)

                .OnProperty(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SpecificationObjects).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededBy).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededBy).IgnoreIt();

            return filler;
        }

        private static DataSet CreateRandomDataSet(string dataSetName) =>
            CreateDataSetFiller(dateTimeOffset: GetRandomDateTimeOffset(), dataSetName).Create();

        private static Filler<DataSet> CreateDataSetFiller(
            DateTimeOffset dateTimeOffset, 
            string dataSetName)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)

                .OnProperty(dataSet => dataSet.DataSetName)
                    .Use(dataSetName)

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