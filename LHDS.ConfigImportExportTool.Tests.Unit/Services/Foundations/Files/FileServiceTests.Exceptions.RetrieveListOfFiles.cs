// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.Files.Exceptions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Theory]
        [MemberData(nameof(FileServiceDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnRetrieveListOfFilesIfDependencyValidationErrorOccursAsync(
                Exception dependencyValidationException)
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    message: "Invalid file service dependency validation error occurred.",
                    innerException: dependencyValidationException);

            var expectedFileDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "File dependency validation error occurred, please contact support.",
                    innerException: invalidFileServiceDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            ObjectColumnProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveListOfFilesIfDependencyErrorOccursAsync(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    message: "Failed file dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(
                    message: "File dependency error occurred, please contact support.",
                    innerException: failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnRetrieveListOfFilesIfServiceErrorOccursAsync()
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(
                    message: "Failed file service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedFileServiceException =
                new FileServiceException(
                    message: "File service error occurred, please contact support.",
                    innerException: failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileServiceException actualException =
                await Assert.ThrowsAsync<FileServiceException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}