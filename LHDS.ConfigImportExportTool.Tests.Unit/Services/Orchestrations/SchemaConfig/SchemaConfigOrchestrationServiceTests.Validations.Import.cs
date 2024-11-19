// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
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
        public async Task ShouldThrowValidationExceptionOnImportIfDataSetNameIsInvalidAsync(string invalidString)
        {
            // given
            Guid inputDataSetId = Guid.NewGuid();
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                specificationObject.ObjectColumns = newObjectColumns;
                inputSpecificationObjects.Add(specificationObject);
            }

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
            ValueTask importTask =
                this.schemaConfigOrchestrationService.Import(
                    inputSpecificationObjects, dataSetName: invalidString, version: invalidString);

            SchemaConfigValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SchemaConfigValidationOrchestrationException>(importTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSchemaConfigValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSchemaConfigValidationOrchestrationException))),
                        Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnImportIfSpecificationObjectListIsNullAsync()
        {
            // given
            List<SpecificationObject> nullSpecificationObject = null;
            string inputVersion = GetRandomString(10);
            string inputDataSetName = GetRandomString(10);
            var nullSpecificationObjectException =
                new NullSpecificationObjectListException(message: "Specification object list is null.");

            var expectedSchemaConfigValidationOrchestrationException =
                new SchemaConfigValidationOrchestrationException(
                    message: "Schema config orchestration validation error occurred, fix the errors and try again.",
                    innerException: nullSpecificationObjectException);

            // when
            ValueTask importTask =
                this.schemaConfigOrchestrationService.Import(
                    nullSpecificationObject, inputDataSetName, inputVersion);

            SchemaConfigValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SchemaConfigValidationOrchestrationException>(importTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSchemaConfigValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSchemaConfigValidationOrchestrationException))),
                        Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
