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
        public async Task ShouldDeleteDirectoryAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            bool recursive = true;

            // when
            await this.fileService.DeleteDirectoryAsync(inputFilePath, recursive);

            // then
            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(inputFilePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
