// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Coordinations.ImportExports
{
    public partial class ImportExportCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldExportObjectsAndColumnAsync()
        {
            // given
            string randomDataSetName = GetRandomString(150);
            string inputDataSetName = randomDataSetName;
            string randomVersion = GetRandomString(10);
            string inputVersion = randomVersion;
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> outputSpecificationObjects = randomSpecificationObjects;
            List<SpecificationObject> inputSpecificationObjects = outputSpecificationObjects;

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Export(inputDataSetName, inputVersion))
                    .ReturnsAsync(outputSpecificationObjects);

            this.readSchemaOrchestrationServiceMock.Setup(service =>
                service.WriteFile(inputSpecificationObjects, inputPath));

            // when
            await this.importExportCoordinationService.Export(
                randomDataSetName, inputVersion, inputPath);

            // then
            this.schemaConfigOrchestrationServiceMock.Verify(service =>
                service.Export(inputDataSetName, inputVersion),
                    Times.Once);

            this.readSchemaOrchestrationServiceMock.Verify(service =>
                service.WriteFile(inputSpecificationObjects, inputPath),
                    Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
