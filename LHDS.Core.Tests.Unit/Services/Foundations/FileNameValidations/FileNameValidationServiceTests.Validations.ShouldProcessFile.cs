// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.FileNameValidation.Exceptions;
using LHDS.Core.Models.Foundations.FileNameValidations.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnShouldProcessFileIfFileNameIsInvalidAsync(
            string invalidFileName)
        {
            // given
            string includePattern = GetRandomString();
            string excludePattern = GetRandomString();

            var invalidArgumentFileNameException =
                new InvalidArgumentFileNameException(
                    message: "Invalid file name validation argument(s), please correct the errors and try again.");

            invalidArgumentFileNameException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedFileNameValidationException =
                new FileNameValidationException(
                    message: "File name validation error occurred, please try again.",
                    innerException: invalidArgumentFileNameException);

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: invalidFileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            FileNameValidationException actualException =
                await Assert.ThrowsAsync<FileNameValidationException>(
                    shouldProcessFileTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedFileNameValidationException);
        }
    }
}