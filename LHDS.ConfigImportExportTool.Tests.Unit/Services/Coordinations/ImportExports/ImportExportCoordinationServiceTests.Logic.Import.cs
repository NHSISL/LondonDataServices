// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldImportObjectsAndColumnAsync()
        {
            // given
            Guid inputDataSetId = Guid.NewGuid();
            string randomDataSetName = GetRandomString(150);
            string inputDataSetName = randomDataSetName;
            string randomVersion = GetRandomString(10);
            string inputVersion = randomVersion;
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;

            this.readSchemaOrchestrationServiceMock.Setup(service =>
                service.ReadFile(inputPath))
                    .ReturnsAsync(storageSpecificationObjects);

            this.schemaConfigOrchestrationServiceMock.Setup(service =>
                service.Import(storageSpecificationObjects, inputDataSetName, inputVersion));

            // when
            await this.importExportCoordinationService.Import(
                randomDataSetName, inputVersion, inputPath);

            // then
            this.readSchemaOrchestrationServiceMock.Verify(service =>
                service.ReadFile(inputPath),
                    Times.Once);

            this.schemaConfigOrchestrationServiceMock.Verify(service =>
                service.Import(storageSpecificationObjects, inputDataSetName, inputVersion),
                    Times.Once);

            this.readSchemaOrchestrationServiceMock.VerifyNoOtherCalls();
            this.schemaConfigOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}