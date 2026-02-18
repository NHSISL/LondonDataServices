// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationServiceTests
    {
        [Fact]
        public async Task ShouldProcessMentalHealthFileWhenMatchesMHPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-MH-S1-Group1";
            string includePattern = @"(?i)-MH-";
            string excludePattern = string.Empty;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessMentalHealthFileWhenDoesNotMatchMHPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-Com-S1-Group1";
            string includePattern = @"(?i)-MH-";
            string excludePattern = string.Empty;
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessMentalHealthFileWithCaseInsensitiveMatchAsync()
        {
            // given
            string fileName = "LDS-NWL-mh-S1-Group1";
            string includePattern = @"(?i)-MH-";
            string excludePattern = string.Empty;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessCommunityFileWhenMatchesComPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-Com-S1-Group1";
            string includePattern = @"(?i)-Com-";
            string excludePattern = string.Empty;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessCommunityFileWhenDoesNotMatchComPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-MH-S1-Group1";
            string includePattern = @"(?i)-Com-";
            string excludePattern = string.Empty;
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessCommunityFileWithCaseInsensitiveMatchAsync()
        {
            // given
            string fileName = "LDS-NWL-com-S1-Group1";
            string includePattern = @"(?i)-Com-";
            string excludePattern = string.Empty;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessOtherFileWhenExcludesBothMHAndComAsync()
        {
            // given
            string fileName = "LDS-NEL-S1-Group2_NewUnits";
            string includePattern = string.Empty;
            string excludePattern = @"(?i)(-MH-|-Com-)";
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessOtherFileWhenContainsMHPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-MH-S1-Group1";
            string includePattern = string.Empty;
            string excludePattern = @"(?i)(-MH-|-Com-)";
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessOtherFileWhenContainsComPatternAsync()
        {
            // given
            string fileName = "LDS-NWL-Com-S1-Group1";
            string includePattern = string.Empty;
            string excludePattern = @"(?i)(-MH-|-Com-)";
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessOtherFileWhenContainsMHWithCaseInsensitiveAsync()
        {
            // given
            string fileName = "LDS-NWL-mh-S1-Group1";
            string includePattern = string.Empty;
            string excludePattern = @"(?i)(-MH-|-Com-)";
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldNotProcessOtherFileWhenContainsComWithCaseInsensitiveAsync()
        {
            // given
            string fileName = "LDS-NWL-com-S1-Group1";
            string includePattern = string.Empty;
            string excludePattern = @"(?i)(-MH-|-Com-)";
            bool expectedResult = false;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessFileWhenNoPatternIsProvidedAsync()
        {
            // given
            string fileName = "LDS-NEL-S1-Group2_NewUnits";
            string includePattern = null;
            string excludePattern = null;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldProcessFileWhenIncludePatternIsEmptyOrWhitespaceAsync(
            string emptyPattern)
        {
            // given
            string fileName = "LDS-NEL-S1-Group2_NewUnits";
            string excludePattern = null;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: emptyPattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldProcessFileWhenExcludePatternIsEmptyOrWhitespaceAsync(
            string emptyPattern)
        {
            // given
            string fileName = "LDS-NWL-Com-S1-Group1";
            string includePattern = null;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: fileName,
                    includePattern: includePattern,
                    excludePattern: emptyPattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ShouldProcessRandomFileWhenNoPatternsAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string includePattern = null;
            string excludePattern = null;
            bool expectedResult = true;

            // when
            ValueTask<bool> shouldProcessFileTask =
                this.fileNameValidationService.ShouldProcessFileAsync(
                    fileName: randomFileName,
                    includePattern: includePattern,
                    excludePattern: excludePattern);

            bool actualResult = await shouldProcessFileTask;

            // then
            actualResult.Should().Be(expectedResult);
        }
    }
}