// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.FileNameValidation.Exceptions;
using LHDS.Core.Models.Foundations.FileNameValidations.Exceptions;
using LHDS.Core.Services.Foundations.FileNameValidations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnShouldProcessFileIfDependencyErrorOccursAsync(
            Exception dependencyValidationException)
        {
            // given
            string someFileName = GetRandomString();
            string someIncludePattern = GetRandomString();
            string someExcludePattern = GetRandomString();

            var fileNameValidationServiceMock = new Mock<FileNameValidationService> { CallBase = true };

            fileNameValidationServiceMock
                .Setup(service => service.MatchesPatterns(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Throws(dependencyValidationException);

            var invalidRegexFileNameValidationException =
                new InvalidRegexFileNameValidationException(
                    message: "Invalid regex pattern occurred, fix errors and try again.",
                    innerException: dependencyValidationException);

            var expectedFileNameValidationDependencyValidationException =
                new FileNameValidationDependencyValidationException(
                    message: "File name validation dependency validation error occurred, fix errors and try again.",
                    innerException: invalidRegexFileNameValidationException);

            // when
            ValueTask<bool> shouldProcessFileTask =
                fileNameValidationServiceMock.Object.ShouldProcessFileAsync(
                    fileName: someFileName,
                    includePattern: someIncludePattern,
                    excludePattern: someExcludePattern);

            FileNameValidationDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileNameValidationDependencyValidationException>(
                    shouldProcessFileTask.AsTask);

            // then
            actualException.Should().BeOfType<FileNameValidationDependencyValidationException>();
            actualException.Message.Should().Be(expectedFileNameValidationDependencyValidationException.Message);
            actualException.InnerException.Should().BeOfType<InvalidRegexFileNameValidationException>();

            fileNameValidationServiceMock.Verify(
                service => service.MatchesPatterns(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            fileNameValidationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnShouldProcessFileIfServiceErrorOccursAsync()
        {
            // given
            string someFileName = GetRandomString();
            string someIncludePattern = GetRandomString();
            string someExcludePattern = GetRandomString();
            var exception = new Exception();

            var fileNameValidationServiceMock = new Mock<FileNameValidationService> { CallBase = true };

            fileNameValidationServiceMock
                .Setup(service => service.MatchesPatterns(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Throws(exception);

            var failedFileNameValidationServiceException =
                new FailedFileNameValidationServiceException(
                    message: "Failed file name validation service error occurred, please contact support.",
                    innerException: exception);

            var expectedFileNameValidationServiceException =
                new FileNameValidationServiceException(
                    message: "File name validation service error occurred, please contact support.",
                    innerException: failedFileNameValidationServiceException);

            // when
            ValueTask<bool> shouldProcessFileTask =
                fileNameValidationServiceMock.Object.ShouldProcessFileAsync(
                    fileName: someFileName,
                    includePattern: someIncludePattern,
                    excludePattern: someExcludePattern);

            FileNameValidationServiceException actualException =
                await Assert.ThrowsAsync<FileNameValidationServiceException>(
                    shouldProcessFileTask.AsTask);

            // then
            actualException.Should().BeOfType<FileNameValidationServiceException>();
            actualException.Message.Should().Be(expectedFileNameValidationServiceException.Message);
            actualException.InnerException.Should().BeOfType<FailedFileNameValidationServiceException>();
            actualException.InnerException.InnerException.Should().BeOfType<Exception>();

            fileNameValidationServiceMock.Verify(
                service => service.MatchesPatterns(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            fileNameValidationServiceMock.VerifyNoOtherCalls();
        }
    }
}