// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnWriteFileIfSpecificationObjectIsNullAndLogItAsync()
        {
            // given
            List<SpecificationObject> nullSpecificationObjects = null;
            string inputPath = GetRandomString();

            var nullSpecificationObjectListException =
                new NullSpecificationObjectListException(message: "Specification object list is null.");

            var expectedReadSchemaValidationOrchestrationException =
                new ReadSchemaValidationOrchestrationException(
                    message: "Read schema orchestration validation error occurred, fix the errors and try again.",
                    innerException: nullSpecificationObjectListException);

            // when
            ValueTask writeSchemaFileTask =
                this.readSchemaOrchestrationService.WriteFile(nullSpecificationObjects, inputPath);

            ReadSchemaValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<ReadSchemaValidationOrchestrationException>(
                    writeSchemaFileTask.AsTask);

            // then
            actualException.Should()
                    .BeEquivalentTo(expectedReadSchemaValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedReadSchemaValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnWriteFileIfPathIsInvalidAsync(string invalidPath)
        {
            // given
            string inputTableName = GetRandomString();
            List<ObjectColumn> objectColumns = CreateRandomObjectColumns(isExport: true);

            List<SpecificationObject> inputSpecificationObjects =
                CreateRandomSpecificationObjects(objectColumns, inputTableName, isExport:true);

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
            ValueTask writeSchemaFileTask =
                this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, invalidPath);

            ReadSchemaValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<ReadSchemaValidationOrchestrationException>(
                    writeSchemaFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedReadSchemaValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedReadSchemaValidationOrchestrationException))),
                        Times.Once);

            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()),
                        Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
