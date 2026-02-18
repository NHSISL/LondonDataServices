// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.FileNameValidation.Exceptions;
using LHDS.Core.Models.Foundations.FileNameValidations.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnShouldProcessFileIfServiceErrorOccursAsync()
        {
            // given
            string someFileName = GetRandomString();
            string someIncludePattern = "[";
            string someExcludePattern = GetRandomString();

            var failedFileNameValidationServiceException =
                new FailedFileNameValidationServiceException(
                    message: "Failed file name validation service error occurred, please contact support.",
                    innerException: new Exception());

            failedFileNameValidationServiceException.AddData(
                key: "InnerException",
                values: "RegexParseException");

            var expectedFileNameValidationServiceException =
                new FileNameValidationServiceException(
                    message: "File name validation service error occurred, please contact support.",
                    innerException: failedFileNameValidationServiceException);

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
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

            actualException.InnerException.InnerException.Should()
                .BeOfType<RegexParseException>();
        }
    }
}