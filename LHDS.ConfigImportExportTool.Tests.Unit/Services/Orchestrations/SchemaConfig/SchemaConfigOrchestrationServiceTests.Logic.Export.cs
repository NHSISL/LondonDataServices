// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldExportObjectsAsync()
        {
            // given
            Guid inputDataSetId = Guid.NewGuid();
            string randomDataSetName = GetRandomString(150);
            string inputDataSetName = randomDataSetName;
            string randomVersion = GetRandomString(10);
            string inputVersion = randomVersion;
            DataSet randomDataSet = CreateRandomDataSet(inputDataSetName);
            randomDataSet.Id = inputDataSetId;

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(inputDataSetId, inputVersion);

            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            randomDataSet.DataSetSpecifications.Add(randomDataSetSpecification);
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            randomSpecificationObjects[0].DataSetSpecificationId = storageDataSetSpecification.Id;
            randomSpecificationObjects[0].DataSetSpecification = storageDataSetSpecification;

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                specificationObject.ObjectColumns = newObjectColumns;
            }

            List<SpecificationObject> storageSpecificationObjects = randomSpecificationObjects;

            List<SpecificationObject> expectedSpecificationObjects = storageSpecificationObjects
                .Where(specificationObject =>
                    specificationObject.DataSetSpecificationId == storageDataSetSpecification.Id).ToList();

            DataSet storageDataSet = randomDataSet;
            List<DataSet> storageDataSets = new List<DataSet> { storageDataSet };

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ReturnsAsync(storageDataSets.AsQueryable());

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjectsAsync())
                    .ReturnsAsync(storageSpecificationObjects.AsQueryable());

            // when
            List<SpecificationObject> actualSpecificationObjects = await this.schemaConfigOrchestrationService
                .Export(randomDataSetName, inputVersion);

            // then
            actualSpecificationObjects.Should().BeEquivalentTo(expectedSpecificationObjects);

            this.dataSetProcessingServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once());

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once());

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
