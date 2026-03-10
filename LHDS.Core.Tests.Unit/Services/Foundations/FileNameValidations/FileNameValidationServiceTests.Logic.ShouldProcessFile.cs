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
        [Theory]
        [InlineData("LDS-NWL-MH-S1-Group1", @"(?i)^[^-]+-[^-]+-MH-", "", true)]
        [InlineData("LDS-NWL-Com-S1-Group1", @"(?i)^[^-]+-[^-]+-MH-", "", false)]
        [InlineData("LDS-NWL-mh-S1-Group1", @"(?i)^[^-]+-[^-]+-MH-", "", true)]
        [InlineData("LDS-MH-NWL-S1-Group1", @"(?i)^[^-]+-[^-]+-MH-", "", false)]
        [InlineData("LDS-NWL-Com-S1-Group1", @"(?i)^[^-]+-[^-]+-Com-", "", true)]
        [InlineData("LDS-NWL-MH-S1-Group1", @"(?i)^[^-]+-[^-]+-Com-", "", false)]
        [InlineData("LDS-NWL-com-S1-Group1", @"(?i)^[^-]+-[^-]+-Com-", "", true)]
        [InlineData("LDS-Com-NWL-S1-Group1", @"(?i)^[^-]+-[^-]+-Com-", "", false)]
        [InlineData("LDS-NEL-S1-Group2_NewUnits", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", true)]
        [InlineData("LDS-NWL-MH-S1-Group1", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", false)]
        [InlineData("LDS-NWL-Com-S1-Group1", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", false)]
        [InlineData("LDS-NWL-mh-S1-Group1", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", false)]
        [InlineData("LDS-NWL-com-S1-Group1", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", false)]
        [InlineData("LDS-MH-NWL-S1-Group1", "", @"(?i)^[^-]+-[^-]+-(MH|Com)-", true)]
        [InlineData("LDS-NEL-S1-Group2_NewUnits", null, null, true)]
        [InlineData("LDS-NEL-S1-Group2_NewUnits", "", null, true)]
        [InlineData("LDS-NEL-S1-Group2_NewUnits", "   ", null, true)]
        [InlineData("LDS-NWL-Com-S1-Group1", null, "", true)]
        [InlineData("LDS-NWL-Com-S1-Group1", null, "   ", true)]
        public async Task ShouldProcessFileAsync(
            string fileName,
            string includePattern,
            string excludePattern,
            bool expectedResult)
        {
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