// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingres
{
    public partial class IngresOrchestrationTests
    {
        [Fact]
        public async Task ShouldCheckForBatchCompleteAndNotCreateBatchReadyFileAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            randomIngestionTracking.DecryptedFileName = CreateRandomDecryptedFilePath();

            string batchReadyFileName =
                $"{Path.GetDirectoryName(randomIngestionTracking.DecryptedFileName)}/BatchReady.txt"
                    .Replace("\\", "/");

            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> ingestionTrackingObjects = GetRandomStringList();
            List<string> dataSetSpecificationObjects = GetRandomStringList();

            Stream batchReadyStream =
                new MemoryStream(Encoding.UTF8.GetBytes(
                    $"All specification object files present for dataset specification id: " +
                    $"{randomIngestionTracking.DataSetSpecificationId}"));

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                .ReturnsAsync(storageIngestionTracking);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference))
                    .ReturnsAsync(ingestionTrackingObjects);

            // when
            await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
            service.AddDocumentAsync(
                It.Is(SameStreamAs(batchReadyStream)),
                batchReadyFileName,
                storageIngestionTracking.Container),
                    Times.Never);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldCheckForBatchCompleteAndCreateBatchReadyFileAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            randomIngestionTracking.DecryptedFileName = CreateRandomDecryptedFilePath();

            string batchReadyFileName =
                $"{Path.GetDirectoryName(randomIngestionTracking.DecryptedFileName)}/BatchReady.txt"
                    .Replace("\\", "/");

            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> randomObjects = GetRandomStringList();
            List<string> ingestionTrackingObjects = randomObjects.DeepClone();
            List<string> dataSetSpecificationObjects = randomObjects.DeepClone();

            Stream batchReadyStream =
                new MemoryStream(Encoding.UTF8.GetBytes(
                    $"All specification object files present for dataset specification id: " +
                    $"{randomIngestionTracking.DataSetSpecificationId}"));

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                .ReturnsAsync(storageIngestionTracking);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference))
                    .ReturnsAsync(ingestionTrackingObjects);

            // when
            await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationId(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
            service.AddDocumentAsync(
                It.Is(SameStreamAs(batchReadyStream)),
                batchReadyFileName,
                storageIngestionTracking.Container),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
