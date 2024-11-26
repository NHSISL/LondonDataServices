// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Clients.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Clients.ImportExports
{
    public partial class ImportExportTests
    {
        [Theory]
        [MemberData(nameof(ImportExportClientDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnExportIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedDependencyValidationException =
                new ImportExportClientDependencyValidationException(
                    message: "Import export client dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.importExportCoordinationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion, inputFilePath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask exportFileTask =
                this.importExportClient.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportClientDependencyValidationException actualException =
                await Assert.ThrowsAsync<ImportExportClientDependencyValidationException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.importExportCoordinationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion, inputFilePath),
                 Times.Once);

            this.importExportCoordinationServiceMock.VerifyNoOtherCalls();
        }
    }
}
