// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
            List<CannonicalSchemaItem> expectedCannonicalSchemaItems = CreateRandomCannonicalSchemaItems();
            IQueryable<SpecificationObject> expectedSpecificationObjects = CreateRandomSpecificationObjects();

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            this.csvHelperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<CannonicalSchemaItem>(inputCsvString, true, fieldMappings))
                    .ReturnsAsync(expectedCannonicalSchemaItems);

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