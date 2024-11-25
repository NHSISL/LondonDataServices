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
            ShouldThrowDependencyValidationOnImportIfDependencyValidationOccursAndLogItAsync(
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

            this.readSchemaOrchestrationServiceMock.Setup(service =>
                service.ReadFile(inputFilePath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask importFileTask =
                this.importExportCoordinationService.Import(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationDependencyValidationException>(
                    importFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.readSchemaOrchestrationServiceMock.Verify(service =>
             service.ReadFile(inputFilePath),
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
            ShouldThrowDependencyExceptionOnImportIfDependencyErrorOccursAndLogItAsync(
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

            this.readSchemaOrchestrationServiceMock.Setup(service =>
                service.ReadFile(inputFilePath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask importFileTask =
                this.importExportCoordinationService.Import(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationDependencyException>(
                    importFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.readSchemaOrchestrationServiceMock.Verify(service =>
             service.ReadFile(inputFilePath),
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
            ShouldThrowServiceExceptionOnImportIfServiceErrorOccursAndLogItAsync()
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

            this.readSchemaOrchestrationServiceMock.Setup(service =>
                service.ReadFile(inputFilePath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask importFileTask =
                this.importExportCoordinationService.Import(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationServiceException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationServiceException>(
                    importFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.readSchemaOrchestrationServiceMock.Verify(service =>
             service.ReadFile(inputFilePath),
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