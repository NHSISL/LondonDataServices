// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
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

            List<SpecificationObject> randomSpecificationObjects =
                CreateRandomSpecificationObjects(randomDateTimeOffset);

            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();
            DataSet storageDataSet = randomDataSet;
            List<DataSet> storageDataSets = new List<DataSet> { storageDataSet };

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns =
                    CreateRandomObjectColumns(randomDateTimeOffset, specificationObject.Id);

                specificationObject.ObjectColumns = newObjectColumns;
                inputSpecificationObjects.Add(specificationObject);
            }

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ReturnsAsync(storageDataSets.AsQueryable());

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffsetAsync())
                        .ReturnsAsync(randomDateTimeOffset);

                this.identifierBrokerMock.Setup(broker =>
                        broker.GetIdentifierAsync())
                            .ReturnsAsync(specificationObject.Id);

                specificationObject.DataSetSpecificationId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertSpecificationObjectAsync(specificationObject))
                        .ReturnsAsync(specificationObject);

                foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                {
                    this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffsetAsync())
                        .ReturnsAsync(randomDateTimeOffset);

                    this.identifierBrokerMock.Setup(broker =>
                        broker.GetIdentifierAsync())
                            .ReturnsAsync(objectColumn.Id);

                    objectColumn.SpecificationObjectId = storageDataSetSpecification.Id;

                    this.objectColumnProcessingServiceMock.Setup(service =>
                        service.ReadOrInsertObjectColumnAsync(objectColumn))
                            .ReturnsAsync(objectColumn);
                };
            };

            // when
            await this.schemaConfigOrchestrationService.Import(
                inputSpecificationObjects, randomDataSetName, inputVersion);

            // then
            this.dataSetProcessingServiceMock.Verify(service =>
                service.RetrieveAllDataSetsAsync(),
                    Times.Once());

            int invocationCount = 0;

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                specificationObject.DataSetSpecificationId = storageDataSetSpecification.Id;

                this.specificationObjectProcessingServiceMock.Verify(service =>
                    service.ReadOrInsertSpecificationObjectAsync(specificationObject),
                        Times.Once());

                invocationCount++;

                foreach (ObjectColumn objectColumn in specificationObject.ObjectColumns)
                {
                    objectColumn.SpecificationObjectId = storageDataSetSpecification.Id;

                    this.objectColumnProcessingServiceMock.Verify(service =>
                        service.ReadOrInsertObjectColumnAsync(objectColumn),
                            Times.Once());

                    invocationCount++;
                };
            };

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(invocationCount));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(invocationCount));

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}