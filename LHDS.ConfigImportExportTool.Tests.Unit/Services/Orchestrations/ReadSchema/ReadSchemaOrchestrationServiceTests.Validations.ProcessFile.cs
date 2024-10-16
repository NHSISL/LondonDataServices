// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfPathIsInvalidAsync(string invalidPath)
        {
            // given
            var invalidArgumentReadSchemaOrchestrationException =
                new InvalidArgumentReadSchemaOrchestrationException(
                    message: "Invalid read schema argument(s), please correct the errors and try again.");

            invalidArgumentReadSchemaOrchestrationException.AddData(
                key: "path",
                values: "Text is required");

            var expectedReadSchemaValidationOrchestrationException =
                new ReadSchemaValidationOrchestrationException(
                    message: "Read schema orchestration validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentReadSchemaOrchestrationException);

            // when
            ValueTask<List<ObjectColumn>> readObjectColumnFromCsv =
                this.readSchemaOrchestrationService.ProcessSchemaFile(invalidPath);

            ReadSchemaValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<ReadSchemaValidationOrchestrationException>(readObjectColumnFromCsv.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedReadSchemaValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedReadSchemaValidationOrchestrationException))),
                        Times.Once);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(invalidPath),
                    Times.Never);

            this.csvHelperServiceMock.Verify(service => 
                service.MapCsvToObjectAsync<ObjectColumn>(
                    It.IsAny<string>(), 
                    It.IsAny<bool>(), 
                    It.IsAny<Dictionary<string, int>>()), 
                        Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }
    }
}
