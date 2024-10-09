// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public async Task ShouldCreateDirectoryAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;

            // when
            await this.fileService.CreateDirectoryAsync(inputFilePath);

            // then
            this.fileBrokerMock.Verify(broker =>
                broker.CreateDirectoryAsync(inputFilePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
