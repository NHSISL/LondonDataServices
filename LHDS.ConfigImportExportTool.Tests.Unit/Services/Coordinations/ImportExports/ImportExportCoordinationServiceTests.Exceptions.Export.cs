// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Coordinations.ImportExports.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Coordinations.ImportExports
{
    public partial class ImportExportCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(ImportExportCoordinationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnExportIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            string inputFilePath = GetRandomString();

            var expectedDependencyValidationException =
                new ImportExportCoordinationDependencyValidationException(
                    message: "Import export coordination dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask exportFileTask =
                this.importExportCoordinationService.Export(inputDataSetName, inputVersion, inputFilePath);

            ImportExportCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ImportExportCoordinationDependencyValidationException>(
                    exportFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.schemaConfigOrchestrationServiceMock.Verify(service =>
             service.Export(inputDataSetName, inputVersion),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyValidationException))),
                       Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
