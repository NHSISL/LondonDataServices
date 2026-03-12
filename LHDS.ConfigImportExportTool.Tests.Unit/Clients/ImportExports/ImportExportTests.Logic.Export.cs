// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Clients.ImportExports
{
    public partial class ImportExportTests
    {
        [Fact]
        public async Task ShouldExport()
        {
            // given
            string someDataSetName = GetRandomString();
            string someVersion = GetRandomString();
            string someFilePath = GetRandomString();
            string inputDataSetName = someDataSetName;
            string inputVersion = someVersion;
            string inputFilePath = someFilePath;

            // when
            await this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            // then
            this.importExportCoordinationServiceMock.Verify(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath),
                    Times.Once);

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
