// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldWriteSchemaCsvFileAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            Dictionary<string, int> fieldMappings = null;
            string randomCsvString = GetRandomString();
            string inputCsvString = randomCsvString;
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                List<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns(isExport: true);

                SpecificationObject randomSpecificationObject =
                    CreateRandomSpecificationObject(randomObjectColumns, tableName: GetRandomString(), isExport: true);

                inputSpecificationObjects.Add(randomSpecificationObject);
            }

            List<CannonicalSchemaItem> inputCannonicalSchemaItems = new List<CannonicalSchemaItem>();

            foreach (var specificationObject in inputSpecificationObjects)
            {
                foreach (var objectColumn in specificationObject.ObjectColumns)
                {
                    CannonicalSchemaItem cannonicalSchemaItem = new CannonicalSchemaItem
                    {
                        TableName = specificationObject.SupplierObjectName,
                        TableDescription = specificationObject.ObjectDescription,
                        ColumnName = objectColumn.SupplierColumnName,
                        ColumnDataType = objectColumn.SqlDataType,
                        ColumnDescription = objectColumn.ColumnDescription,
                        ColumnLength = objectColumn.Length,
                        ColumnOrdinal = objectColumn.OrdinalPosition,
                        LinkedTable = objectColumn.ForeignKeyTableName,
                        LinkedColumn = objectColumn.ForeignKeyColumnName,
                        OurObjectName = specificationObject.OurObjectName,
                        InterchangeProtocol = specificationObject.InterchangeProtocol,
                        IsPushedToUs = specificationObject.IsPushedToUs,
                        IsPulledByUs = specificationObject.IsPulledByUs,
                        DeletionHandling = specificationObject.DeletionHandling,
                        IsSubmissionHeaderObject = specificationObject.IsSubmissionHeaderObject,
                        IsTransactionLog = specificationObject.IsTransactionLog,
                        OurColumnName = objectColumn.OurColumnName,
                        PopulatedBy = objectColumn.PopulatedBy,
                        FhirDataType = objectColumn.FhirDataType,
                        Precision = objectColumn.Precision,
                        Scale = objectColumn.Scale,
                        SupplierDateFormat = objectColumn.SupplierDateFormat,
                        IsWatermark = objectColumn.IsWatermark,
                        IsSequencing = objectColumn.IsSequencing,
                        IsBusinessKey = objectColumn.IsBusinessKey,
                        IsUniqueRecordKey = objectColumn.IsUniqueRecordKey,
                        IsVersionHashElement = objectColumn.IsVersionHashElement,
                        IsSenderCode = objectColumn.IsSenderCode,
                        IsAuthorCode = objectColumn.IsAuthorCode,
                        IsRelatedOrganisationId = objectColumn.IsRelatedOrganisationId,
                        IsDeleteFlag = objectColumn.IsDeleteFlag,
                        IsSensitiveRecordMarker = objectColumn.IsSensitiveRecordMarker,
                        IsPersonConfidentialData = objectColumn.IsPersonConfidentialData,
                        PersonConfidentialDataType = objectColumn.PersonConfidentialDataType,
                        MaskingMethod = objectColumn.MaskingMethod,
                        CodeSystem = objectColumn.CodeSystem,
                        PartitionColumnLevel = objectColumn.PartitionColumnLevel,
                        IsForeignKey = objectColumn.IsForeignKey,
                    };

                    inputCannonicalSchemaItems.Add(cannonicalSchemaItem);
                }
            }

            this.csvHelperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.Is(SameCannonicalSchemaItemListAs(inputCannonicalSchemaItems)), true, fieldMappings, false))
                        .ReturnsAsync(inputCsvString);

            this.fileServiceMock.Setup(service =>
                service.WriteToFileAsync(inputFilePath, randomCsvString))
                    .ReturnsAsync(true);

            // when
            await this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, inputFilePath);

            // then
            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.Is(SameCannonicalSchemaItemListAs(inputCannonicalSchemaItems)), true, fieldMappings, false),
                        Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(inputFilePath, inputCsvString),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}