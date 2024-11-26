// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Clients;
using LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Clients.ImportExports
{
    public partial class ImportExportTests
    {
        private readonly Mock<IImportExportCoordinationService> importExportCoordinationServiceMock;
        private readonly IImportExportClient importExportClient;

        public ImportExportTests()
        {
            this.importExportCoordinationServiceMock = new Mock<IImportExportCoordinationService>();
            this.importExportClient = new ImportExportClient(this.importExportCoordinationServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
