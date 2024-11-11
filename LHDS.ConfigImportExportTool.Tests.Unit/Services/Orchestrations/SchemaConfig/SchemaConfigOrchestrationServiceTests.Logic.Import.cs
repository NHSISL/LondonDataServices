// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.DatasetSpecifications;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldImportObjectsAsync()
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
            List<SpecificationObject> specificationObjects = CreateRandomSpecificationObjects(inputDataSetId);
            List<ObjectColumn> objectColumns = new List<ObjectColumn>();

            foreach (SpecificationObject specificationObject in specificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                objectColumns.AddRange(newObjectColumns);
            }

            SchemaConfig randomSchemaConfig = CreateRandomSchemaConfig(
                randomDataSet.Id, specificationObjects, objectColumns);

            SchemaConfig inputSchemaConfig = randomSchemaConfig;
            List<DataSet> storageDataSets = new List<DataSet> { randomDataSet };

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ReturnsAsync(storageDataSets.AsQueryable());

            foreach(SpecificationObject specificationObject in inputSchemaConfig.SpecificationObjects) 
            {
                specificationObject.DataSetSpecificationId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertSpecificationObjectAsync(specificationObject))
                        .ReturnsAsync(specificationObject);
            };

            foreach (ObjectColumn objectColumn in inputSchemaConfig.ObjectColumns)
            {
                objectColumn.SpecificationObjectId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Setup(service =>
                    service.RetrieveAllSpecificationObjectsAsync()).
                        ReturnsAsync(specificationObjects.AsQueryable());

                this.objectColumnProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertObjectColumnAsync(objectColumn))
                        .ReturnsAsync(objectColumn);
            };

            // when
            await this.schemaConfigOrchestrationService.Import(inputSchemaConfig, randomDataSetName, inputVersion);

            // then
            this.dataSetProcessingServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(), 
                    Times.Once());

            foreach (SpecificationObject specificationObject in inputSchemaConfig.SpecificationObjects)
            {
                specificationObject.DataSetSpecificationId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Verify(service =>
                    service.ReadOrInsertSpecificationObjectAsync(specificationObject), 
                        Times.Once());
            };

            foreach (ObjectColumn objectColumn in inputSchemaConfig.ObjectColumns)
            {
                objectColumn.SpecificationObjectId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Verify(service =>
                    service.RetrieveAllSpecificationObjectsAsync(), 
                        Times.Once());

                this.objectColumnProcessingServiceMock.Verify(service =>
                    service.ReadOrInsertObjectColumnAsync(objectColumn),
                        Times.Once());
            };

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}