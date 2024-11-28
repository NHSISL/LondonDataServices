// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnExportIfDataSetNameIsInvalidAsync(string invalidString)
        {
            // given
            var invalidArgumentSchemaConfigOrchestrationException =
                new InvalidArgumentSchemaConfigOrchestrationException(
                    message: "Invalid schema config argument(s), please correct the errors and try again.");

            invalidArgumentSchemaConfigOrchestrationException.AddData(
                key: "dataSetName",
                values: "Text is required");

            invalidArgumentSchemaConfigOrchestrationException.AddData(
                key: "version",
                values: "Text is required");

            var expectedSchemaConfigValidationOrchestrationException =
                new SchemaConfigValidationOrchestrationException(
                    message: "Schema config orchestration validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentSchemaConfigOrchestrationException);

            // when
            ValueTask<List<SpecificationObject>> exportTask =
                this.schemaConfigOrchestrationService.Export(dataSetName: invalidString, version: invalidString);

            SchemaConfigValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SchemaConfigValidationOrchestrationException>(exportTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSchemaConfigValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSchemaConfigValidationOrchestrationException))),
                        Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
