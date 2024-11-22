// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Coordinations.ImportExports
{
    public partial class ImportExportCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(ImportExportCoordinationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnExportIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedDependencyValidationException =
                new ImportExportCoordinationDependencyValidationException(
                    message: "Import export coordination dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask exportFileTask =
                this.importExportCoordinationService.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationDependencyValidationException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.schemaConfigOrchestrationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyValidationException))),
                       Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ImportExportCoordinationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnExportIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedDependencyException =
                new ImportExportCoordinationDependencyException(
                    message: "Import export coordination dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask exportFileTask =
                this.importExportCoordinationService.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationDependencyException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.schemaConfigOrchestrationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnExportIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();
            var serviceException = new Exception();

            var failedImportExportCoordinationServiceException =
                new FailedImportExportCoordinationServiceException(
                    message: "Failed import export coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedServiceException =
                new ImportExportCoordinationServiceException(
                    message: "Import export coordination service error occurred, " +
                        "please contact support.",
                    innerException: failedImportExportCoordinationServiceException);

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask exportFileTask =
                this.importExportCoordinationService.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationServiceException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationServiceException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.schemaConfigOrchestrationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedServiceException))),
                       Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
