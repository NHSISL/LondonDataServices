// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(SchemaConfigOrchestrationExportDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnExportIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);

            var expectedDependencyException =
                new SchemaConfigOrchestrationDependencyValidationException(
                    message: "Schema config orchestration dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<SpecificationObject>> exportTask =
                this.schemaConfigOrchestrationService.Export(inputDataSetName, inputVersion);

            SchemaConfigOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<SchemaConfigOrchestrationDependencyValidationException>(
                    exportTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.dataSetProcessingServiceMock.Verify(service =>
             service.RetrieveAllDataSetsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SchemaConfigOrchestrationExportDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnExportIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);

            var expectedDependencyException =
                new SchemaConfigOrchestrationDependencyException(
                    message: "Schema config orchestration dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<SpecificationObject>> exportTask =
                this.schemaConfigOrchestrationService.Export(inputDataSetName, inputVersion);

            SchemaConfigOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<SchemaConfigOrchestrationDependencyException>(
                    exportTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.dataSetProcessingServiceMock.Verify(service =>
             service.RetrieveAllDataSetsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
