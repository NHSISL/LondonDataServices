// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Clients.ImportExports;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using LHDS.ConfigImportExportTool.Services.Coordinations.ImportExports;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        private static string GetRandomString(int length) =>
           new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        public static TheoryData<Xeption> ImportExportClientDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ImportExportCoordinationValidationException(
                    message: "Import export coordination validation error occurred, fix errors and try again.",
                    innerException),

                new ImportExportCoordinationDependencyValidationException(
                    message: "Import export coordination dependency validation error occurred, please contact support.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> ImportExportClientDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ImportExportCoordinationDependencyException(
                    message: "Import export coordination dependency error occurred, please contact support.",
                    innerException)
            };
        }
    }
}
