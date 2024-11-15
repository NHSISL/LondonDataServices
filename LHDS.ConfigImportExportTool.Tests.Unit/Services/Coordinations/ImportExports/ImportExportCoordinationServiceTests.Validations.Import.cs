// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Coordinations.ImportExports
{
    public partial class ImportExportCoordinationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnImportIfDataSetNameIsInvalidAsync(string invalidString)
        {
            // given
            var invalidArgumentImportExportCoordinationException =
                new InvalidArgumentImportExportCoordinationException(
                    message:
                        "Invalid import export coordination argument(s), please correct the errors and try again.");

            invalidArgumentImportExportCoordinationException.AddData(
                key: "dataSetName",
                values: "Text is required");

            invalidArgumentImportExportCoordinationException.AddData(
                key: "version",
                values: "Text is required");

            invalidArgumentImportExportCoordinationException.AddData(
                key: "filePath",
                values: "Text is required");

            var expectedImportExportValidationOrchestrationException =
                new ImportExportValidationCoordinationException(
                    message: "Import export coordination validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentImportExportCoordinationException);

            // when
            ValueTask importTask = this.importExportCoordinationService.Import(
                    dataSetName: invalidString,
                    version: invalidString,
                    filePath: invalidString);

            ImportExportValidationCoordinationException actualException =
                await Assert.ThrowsAsync<ImportExportValidationCoordinationException>(importTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedImportExportValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImportExportValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
