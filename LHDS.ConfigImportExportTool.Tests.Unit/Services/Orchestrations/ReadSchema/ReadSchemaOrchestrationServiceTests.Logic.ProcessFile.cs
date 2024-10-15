// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
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
            List<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            List<ObjectColumn> expectedObjectColumns = randomObjectColumns;

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            this.csvHelperServiceMock.Setup(service =>
                service.MapCsvToObjectAsync<ObjectColumn>(inputCsvString, true, fieldMappings))
                    .ReturnsAsync(expectedObjectColumns.ToList);

            // when
            List<ObjectColumn> actualObjectColumn = 
                await this.readSchemaOrchestrationService.ProcessSchemaFile(inputFilePath);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumns);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputFilePath),
                    Times.Once);

            this.csvHelperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<ObjectColumn>(inputCsvString, true, fieldMappings)
                    ,Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }
    }
}