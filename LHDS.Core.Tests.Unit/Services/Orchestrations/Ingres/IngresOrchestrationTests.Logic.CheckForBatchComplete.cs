// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingres
{
    public partial class IngresOrchestrationTests
    {
        public async Task ShouldCheckForBatchCompleteAndNotCreateBatchReadyFileAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> ingestionTrackingObjects = GetRandomStringList();
            List<string> dataSetSpecificationObjects = GetRandomStringList();

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                .ReturnsAsync(storageIngestionTracking);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference))
                    .ReturnsAsync(ingestionTrackingObjects);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            // when
            await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
