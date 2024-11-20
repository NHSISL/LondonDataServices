// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Text;
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
            byte[] outputResult = ASCIIEncoding.UTF8.GetBytes(inputCsvString);
            byte[] expectedBytes = outputResult;
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                List<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();

                SpecificationObject randomSpecificationObject =
                    CreateRandomSpecificationObject(randomObjectColumns, tableName: GetRandomString());

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
                        ColumnName = objectColumn.SupplierColumnName,
                        ColumnDataType = objectColumn.SqlDataType,
                        ColumnDescription = objectColumn.ColumnDescription,
                        ColumnLength = objectColumn.Length,
                        ColumnOrdinal = objectColumn.OrdinalPosition,
                        LinkedTable = objectColumn.ForeignKeyTableName,
                        LinkedColumn = objectColumn.ForeignKeyColumnName,
                    };

                    inputCannonicalSchemaItems.Add(cannonicalSchemaItem);
                }
            }

            this.csvHelperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    inputCannonicalSchemaItems, true, fieldMappings, false))
                        .ReturnsAsync(inputCsvString);

            this.fileServiceMock.Setup(service =>
                service.WriteToFileAsync(inputFilePath, randomCsvString))
                    .ReturnsAsync(true);

            // when
            await this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, inputFilePath);

            // then
            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    inputCannonicalSchemaItems, true, fieldMappings, false),
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