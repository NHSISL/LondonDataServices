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

            DataSet storageDataSet = randomDataSet;

            List<SpecificationObject> specificationObjects = CreateRandomSpecificationObjects(inputDataSetId); 

            SchemaConfig randomSchemaConfig = CreateRandomSchemaConfig(
                storageDataSet.Id, );

            SchemaConfig inputSchemaConfig = randomSchemaConfig;
            List<DataSet> storageDataSets = new List<DataSet> { storageDataSet };

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

                this.objectColumnProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertObjectColumnAsync(objectColumn))
                        .ReturnsAsync(objectColumn);
            };

            // when
            await this.schemaConfigOrchestrationService.Import(inputSchemaConfig, randomDataSetName, inputVersion);

            // then
            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.ReadOrInsertSpecificationObjectAsync(),
                    Times.Once);

            this.csvHelperServiceMock.Verify(service =>
                service.MapCsvToObjectAsync<ObjectColumn>(inputCsvString, true, fieldMappings),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }
    }
}