// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldProcessSchemaFileAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            Dictionary<string, int> fieldMappings = null;
            string randomCsvString = GetRandomString();
            string inputCsvString = randomCsvString;
            byte[] outputResult = ASCIIEncoding.UTF8.GetBytes(inputCsvString);
            byte[] expectedBytes = outputResult;
            List<SpecificationObject> expectedSpecificationObjects = new List<SpecificationObject>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                List<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();

                List<SpecificationObject> randomSpecificationObjects =
                    CreateRandomSpecificationObjects(randomObjectColumns, tableName: GetRandomString());

                expectedSpecificationObjects.AddRange(randomSpecificationObjects);
            }

            List<CannonicalSchemaItem> randomCannonicalSchemaItems = new List<CannonicalSchemaItem>();

            foreach (var specificationObject in expectedSpecificationObjects)
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

                    randomCannonicalSchemaItems.Add(cannonicalSchemaItem);
                }
            }

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            this.csvHelperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<CannonicalSchemaItem>(inputCsvString, true, fieldMappings))
                    .ReturnsAsync(randomCannonicalSchemaItems);

            // when
            List<SpecificationObject> actualObjectColumn =
                await this.readSchemaOrchestrationService.ReadFile(inputFilePath);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputFilePath),
                    Times.Once);

            this.csvHelperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<CannonicalSchemaItem>(inputCsvString, true, fieldMappings),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }
    }
}