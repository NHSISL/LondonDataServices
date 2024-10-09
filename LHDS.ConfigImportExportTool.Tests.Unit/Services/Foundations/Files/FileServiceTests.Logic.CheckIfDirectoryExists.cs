// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public async Task ShouldCheckIfDirectoryExistsAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            bool outputResult = true;
            bool expectedResult = outputResult;

            this.fileBrokerMock.Setup(broker =>
                broker.CheckIfDirectoryExistsAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            // when
            bool actualResult = await this.fileService
                .CheckIfDirectoryExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileBrokerMock.Verify(broker =>
                broker.CheckIfDirectoryExistsAsync(inputFilePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
