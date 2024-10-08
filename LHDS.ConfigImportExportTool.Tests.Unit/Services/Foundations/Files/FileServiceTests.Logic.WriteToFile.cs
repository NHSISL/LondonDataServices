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
        public async Task ShouldWriteToFileAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string randomContent = GetRandomString();
            string inputContent = randomContent;

            // when
            await this.fileService.WriteToFileAsync(inputFilePath, inputContent);

            // then
            this.fileBrokerMock.Verify(broker =>
                broker.WriteToFileAsync(inputFilePath, inputContent),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}