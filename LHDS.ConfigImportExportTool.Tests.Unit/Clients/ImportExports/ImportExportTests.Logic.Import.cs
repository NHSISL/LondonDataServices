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
        public async Task ShouldImport()
        {
            // Given
            string someDataSetName = GetRandomString();
            string someVersion = GetRandomString();
            string someFilePath = GetRandomString();
            string inputDataSetName = someDataSetName;
            string inputVersion = someVersion;
            string inputFilePath = someFilePath;

            // When
            await this.importExportClient.Import(inputDataSetName, inputVersion, inputFilePath);

            // Then

            this.importExportCoordinationServiceMock.Verify(service =>
                service.Import(inputDataSetName, inputVersion, inputFilePath),
                    Times.Once());

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
