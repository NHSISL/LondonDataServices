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
            // Given
            string someDataSetName = GetRandomString();
            string someVersion = GetRandomString();
            string someFilePath = GetRandomString();
            string inputDataSetName = someDataSetName;
            string inputVersion = someVersion;
            string inputFilePath = someFilePath;

            // When
            await this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            // Then

            this.importExportCoordinationServiceMock.Verify(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath),
                    Times.Once());

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
