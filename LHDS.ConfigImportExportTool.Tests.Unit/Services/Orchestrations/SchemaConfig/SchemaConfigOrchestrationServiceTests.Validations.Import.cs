// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
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
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnImportIfDataSetNameIsInvalidAsync(string invalidDataSetName)
        {
            // given
            string validVersion = GetRandomString(10);
            Guid inputDataSetId = Guid.NewGuid();
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects(inputDataSetId);
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                specificationObject.ObjectColumns = newObjectColumns;
                inputSpecificationObjects.Add(specificationObject);
            }

            var invalidArgumentReadSchemaOrchestrationException =
                new InvalidArgumentSchemaConfigOrchestrationException(
                    message: "Invalid schema config argument(s), please correct the errors and try again.");

            invalidArgumentReadSchemaOrchestrationException.AddData(
                key: "dataSetName",
                values: "Text is required");

            var expectedReadSchemaValidationOrchestrationException =
                new ReadSchemaValidationOrchestrationException(
                    message: "SchemaConfig orchestration validation error occurred, fix the errors and try again.",
                    innerException: invalidArgumentReadSchemaOrchestrationException);

            // when
            ValueTask importTask =
                this.schemaConfigOrchestrationService.Import(
                    inputSpecificationObjects, invalidDataSetName, validVersion);

            ReadSchemaValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<ReadSchemaValidationOrchestrationException>(importTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedReadSchemaValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedReadSchemaValidationOrchestrationException))),
                        Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
