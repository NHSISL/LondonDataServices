// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Clients.Exceptions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Clients.ImportExports
{
    public partial class ImportExportTests
    {
        [Theory]
        [MemberData(nameof(ImportExportClientDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnExportIfDependencyValidationOccurs(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedValidationException =
                new ImportExportClientValidationException(
                    message: "Import export client validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.importExportCoordinationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask exportFileTask =
                this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportClientValidationException actualException =
                await Assert.ThrowsAsync<ImportExportClientValidationException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedValidationException);

            this.importExportCoordinationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion, inputFilePath),
                 Times.Once);

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ImportExportClientDependencyExceptions))]
        public async Task
            ShouldThrowDependencyErrorOnExportIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedDependencyException =
                new ImportExportClientDependencyException(
                    message: "Import export client dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.importExportCoordinationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask exportFileTask =
                this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportClientDependencyException actualException =
                await Assert.ThrowsAsync<ImportExportClientDependencyException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.importExportCoordinationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion, inputFilePath),
                 Times.Once);

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowErrorOnExportIfErrorOccurs()
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();
            var serviceException = new Exception();

            var importExportCoordinationServiceException =
                new ImportExportCoordinationServiceException(
                    message: "Import export coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedServiceException =
                new ImportExportClientServiceException(
                    message: "Import export client service error occurred, " +
                        "fix errors and try again.",
                    innerException: importExportCoordinationServiceException.InnerException as Xeption);

            this.importExportCoordinationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath))
                    .ThrowsAsync(importExportCoordinationServiceException);

            // when
            ValueTask exportFileTask =
                this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportClientServiceException actualException =
                await Assert.ThrowsAsync<ImportExportClientServiceException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.importExportCoordinationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion, inputFilePath),
                 Times.Once);

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
