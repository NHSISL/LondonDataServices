// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Brokers.Files;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Clients.ImportExports;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance.Clients.ImportExports
{
    public partial class ImportExportClientTests
    {
        private readonly IStorageBroker storageBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IFileBroker fileBroker;
        private readonly IImportExportClient importExportClient;

        public ImportExportClientTests()
        {
            ImportExportClient importExportClient = new ImportExportClient();
            this.importExportClient = importExportClient;
            storageBroker = importExportClient.StorageBroker;
            this.csvHelperBroker = new CsvHelperBroker();
            this.fileBroker = new FileBroker();
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

        public static bool GetRandomBoolean()
        {
            Random rand = new Random();

            return rand.Next(2) == 0;
        }

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSetSpecification CreateRandomDataSetSpecification(Guid dataSetId) =>
            CreateDataSetSpecificationFiller(dataSetId).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(Guid dataSetId)
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
                    dataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

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
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }

        private static dynamic CreateRandomDynamicSchemaItem()
        {
            return new
            {
                TableName = GetRandomString(),
                OurObjectName = GetRandomString(),
                TableDescription = GetRandomString(),
                InterchangeProtocol = GetRandomString(),
                IsPushedToUs = GetRandomBoolean(),
                IsPulledByUs = GetRandomBoolean(),
                DeletionHandling = GetRandomString(),
                IsSubmissionHeaderObject = GetRandomBoolean(),
                IsTransactionLog = GetRandomBoolean(),
                ColumnName = GetRandomString(),
                OurColumnName = GetRandomString(),
                ColumnDescription = GetRandomString(),
                ColumnOrdinal = GetRandomNumber(),
                PopulatedBy = GetRandomString(),
                FhirDataType = GetRandomString(10),
                SqlDataType = GetRandomString(10),
                Length = GetRandomNumber(),
                Precision = GetRandomNumber(),
                Scale = GetRandomNumber(),
                SupplierDateFormat = GetRandomString(),
                IsWatermark = GetRandomBoolean(),
                IsSequencing = GetRandomBoolean(),
                IsBusinessKey = GetRandomBoolean(),
                IsUniqueRecordKey = GetRandomBoolean(),
                IsVersionHashElement = GetRandomBoolean(),
                IsSenderCode = GetRandomBoolean(),
                IsAuthorCode = GetRandomBoolean(),
                IsRelatedOrganisationId = GetRandomBoolean(),
                IsDeleteFlag = GetRandomBoolean(),
                IsSensitiveRecordMarker = GetRandomBoolean(),
                IsPersonConfidentialData = GetRandomBoolean(),
                PersonConfidentialDataType = GetRandomString(),
                MaskingMethod = GetRandomString(),
                CodeSystem = GetRandomString(),
                PartitionColumnLevel = GetRandomString(),
                IsForeignKey = GetRandomBoolean(),
                LinkedTable = GetRandomString(),
                LinkedColumn = GetRandomString()
            };
        }

        private static CannonicalSchemaItem CreateCannonicalSchemaItemFromDynamic(dynamic schemaItem)
        {
            return new CannonicalSchemaItem
            {
                TableName = schemaItem.TableName,
                OurObjectName = schemaItem.OurObjectName,
                TableDescription = schemaItem.TableDescription,
                InterchangeProtocol = schemaItem.InterchangeProtocol,
                IsPushedToUs = schemaItem.IsPushedToUs,
                IsPulledByUs = schemaItem.IsPulledByUs,
                DeletionHandling = schemaItem.DeletionHandling,
                IsSubmissionHeaderObject = schemaItem.IsSubmissionHeaderObject,
                IsTransactionLog = schemaItem.IsTransactionLog,
                ColumnName = schemaItem.ColumnName,
                OurColumnName = schemaItem.OurColumnName,
                ColumnDescription = schemaItem.ColumnDescription,
                ColumnOrdinal = schemaItem.ColumnOrdinal,
                PopulatedBy = schemaItem.PopulatedBy,
                FhirDataType = schemaItem.FhirDataType,
                ColumnDataType = schemaItem.SqlDataType,
                ColumnLength = schemaItem.Length,
                Precision = schemaItem.Precision,
                Scale = schemaItem.Scale,
                SupplierDateFormat = schemaItem.SupplierDateFormat,
                IsWatermark = schemaItem.IsWatermark,
                IsSequencing = schemaItem.IsSequencing,
                IsBusinessKey = schemaItem.IsBusinessKey,
                IsUniqueRecordKey = schemaItem.IsUniqueRecordKey,
                IsVersionHashElement = schemaItem.IsVersionHashElement,
                IsSenderCode = schemaItem.IsSenderCode,
                IsAuthorCode = schemaItem.IsAuthorCode,
                IsRelatedOrganisationId = schemaItem.IsRelatedOrganisationId,
                IsDeleteFlag = schemaItem.IsDeleteFlag,
                IsSensitiveRecordMarker = schemaItem.IsSensitiveRecordMarker,
                IsPersonConfidentialData = schemaItem.IsPersonConfidentialData,
                PersonConfidentialDataType = schemaItem.PersonConfidentialDataType,
                MaskingMethod = schemaItem.MaskingMethod,
                CodeSystem = schemaItem.CodeSystem,
                PartitionColumnLevel = schemaItem.PartitionColumnLevel,
                IsForeignKey = schemaItem.IsForeignKey,
                LinkedTable = schemaItem.LinkedTable,
                LinkedColumn = schemaItem.LinkedColumn
            };
        }

        private static SpecificationObject CreateSpecificationObjectFromDynamic(
            dynamic schemaItem,
            Guid dataSetSpecificationId)
        {
            string user = GetRandomString();
            DateTimeOffset currentDateTime = DateTimeOffset.Now;

            SpecificationObject randomSpecificationObject = new SpecificationObject
            {
                Id = Guid.NewGuid(),
                DataSetSpecificationId = dataSetSpecificationId,
                SupplierObjectName = schemaItem.TableName,
                OurObjectName = schemaItem.OurObjectName,
                ObjectDescription = schemaItem.TableDescription,
                InterchangeProtocol = schemaItem.InterchangeProtocol,
                IsPushedToUs = schemaItem.IsPushedToUs,
                IsPulledByUs = schemaItem.IsPulledByUs,
                DeletionHandling = schemaItem.DeletionHandling,
                IsSubmissionHeaderObject = schemaItem.IsSubmissionHeaderObject,
                IsTransactionLog = schemaItem.IsTransactionLog,
                CreatedBy = user,
                UpdatedBy = user,
                UpdatedDate = currentDateTime,
                CreatedDate = currentDateTime,
            };

            return randomSpecificationObject;
        }

        private static ObjectColumn CreateObjectColumnFromDynamic(
            dynamic schemaItem,
            Guid specificationObjectId)
        {
            string user = GetRandomString();
            DateTimeOffset currentDateTime = DateTimeOffset.Now;

            ObjectColumn randomObjectColumn = new ObjectColumn
            {
                Id = Guid.NewGuid(),
                SpecificationObjectId = specificationObjectId,
                SupplierColumnName = schemaItem.ColumnName,
                OurColumnName = schemaItem.OurColumnName,
                ColumnDescription = schemaItem.ColumnDescription,
                OrdinalPosition = schemaItem.ColumnOrdinal,
                PopulatedBy = schemaItem.PopulatedBy,
                FhirDataType = schemaItem.FhirDataType,
                SqlDataType = schemaItem.SqlDataType,
                Length = schemaItem.Length,
                Precision = schemaItem.Precision,
                Scale = schemaItem.Scale,
                SupplierDateFormat = schemaItem.SupplierDateFormat,
                IsWatermark = schemaItem.IsWatermark,
                IsSequencing = schemaItem.IsSequencing,
                IsBusinessKey = schemaItem.IsBusinessKey,
                IsUniqueRecordKey = schemaItem.IsUniqueRecordKey,
                IsVersionHashElement = schemaItem.IsVersionHashElement,
                IsSenderCode = schemaItem.IsSenderCode,
                IsAuthorCode = schemaItem.IsAuthorCode,
                IsRelatedOrganisationId = schemaItem.IsRelatedOrganisationId,
                IsDeleteFlag = schemaItem.IsDeleteFlag,
                IsSensitiveRecordMarker = schemaItem.IsSensitiveRecordMarker,
                IsPersonConfidentialData = schemaItem.IsPersonConfidentialData,
                PersonConfidentialDataType = schemaItem.PersonConfidentialDataType,
                MaskingMethod = schemaItem.MaskingMethod,
                CodeSystem = schemaItem.CodeSystem,
                PartitionColumnLevel = schemaItem.PartitionColumnLevel,
                DataTypeId = Guid.NewGuid(),
                IsForeignKey = schemaItem.IsForeignKey,
                ForeignKeyTableName = schemaItem.LinkedTable,
                ForeignKeyColumnName = schemaItem.LinkedColumn,
                CreatedBy = user,
                UpdatedBy = user,
                UpdatedDate = currentDateTime,
                CreatedDate = currentDateTime,
            };

            return randomObjectColumn;
        }

        private static List<ObjectColumn> CreateRandomObjectColumns(DateTimeOffset dateTimeOffset, Guid specificationId)
        {
            return CreateObjectColumnFiller(dateTimeOffset, specificationId)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

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

        private static List<SpecificationObject> CreateRandomSpecificationObjects(
            Guid dataSetSpecificationId)
        {
            return CreateSpecificationObjectFiller(dateTimeOffset: GetRandomDateTimeOffset(), dataSetSpecificationId)
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static Filler<SpecificationObject> CreateSpecificationObjectFiller(
            DateTimeOffset dateTimeOffset,
            Guid dataSetSpecificationId)
        {
            string user = GetRandomString(255);
            var filler = new Filler<SpecificationObject>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(specificationObject => specificationObject.DataSetSpecificationId).Use(dataSetSpecificationId)
                .OnProperty(specificationObject => specificationObject.ObjectColumns).IgnoreIt()
                .OnProperty(specificationObject => specificationObject.DataSetSpecification).IgnoreIt();

            return filler;
        }
    }
}