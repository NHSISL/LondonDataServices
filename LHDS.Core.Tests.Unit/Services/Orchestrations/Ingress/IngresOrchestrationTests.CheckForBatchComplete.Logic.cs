// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Ingress;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationTests
    {
        [Fact]
        public async Task ShouldCheckForBatchCompleteAndNotCreateBatchReadyFileAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            randomIngestionTracking.DecryptedFileName = CreateRandomDecryptedFilePath();
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> dataSetSpecificationObjects = GetRandomStringList();
            List<string> ingestionTrackingObjects = dataSetSpecificationObjects.DeepClone();
            ingestionTrackingObjects.Remove(dataSetSpecificationObjects.First());

            string message =
                $"Checking IngestionTrackingId {ingestionTrackingId} for subscriber agreement " +
                $"'{randomIngestionTracking.SubscriberAgreementId}' and batch '{randomIngestionTracking.Batch}' " +
                $"Batch is not complete. " +
                $"Missing specification object files: {string.Join(", ", dataSetSpecificationObjects.First())}";

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                .ReturnsAsync(storageIngestionTracking);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference,
                    storageIngestionTracking.SubscriberAgreementId.Value,
                    true))
                        .ReturnsAsync(ingestionTrackingObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.MarkAsBatchCompleteAsync(ingestionTrackingId, false))
                    .Returns(ValueTask.CompletedTask);

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            {
                CallBase = true
            };

            // when
            await ingressOrchestrationServiceMock.Object.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference,
                    storageIngestionTracking.SubscriberAgreementId.Value,
                    true),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.MarkAsBatchCompleteAsync(ingestionTrackingId, false),
                    Times.Once);

            this.loggingBrokerMock.Verify(service =>
                service.LogInformationAsync(message),
                    Times.Once);

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
            string batchReadyFileName = this.landingConfiguration.BatchReadyFile;
            string batchReadyFilePath = $"{randomIngestionTracking.BatchReadyFolderPath}/{batchReadyFileName}";

            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> randomObjects = GetRandomStringList();
            List<string> ingestionTrackingObjects = randomObjects.DeepClone();
            List<string> dataSetSpecificationObjects = randomObjects.DeepClone();

            string message =
                $"All specification object files present for subscriber agreement " +
                $"'{randomIngestionTracking.SubscriberAgreementId}' and batch '{randomIngestionTracking.Batch}' " +
                $"as defined in Dataset Specification Id: '{randomIngestionTracking.DataSetSpecificationId}'.";

            Stream batchReadyStream =
                new MemoryStream(Encoding.UTF8.GetBytes(message));

            Stream expectedStream = batchReadyStream.DeepClone();

            Stream actualStream = new MemoryStream();

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                    .ReturnsAsync(storageIngestionTracking);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference,
                    storageIngestionTracking.SubscriberAgreementId.Value,
                    true))
                        .ReturnsAsync(ingestionTrackingObjects);

            var ingressOrchestrationServiceMock = new Mock<IngressOrchestrationService>(
                this.ingestionTrackingProcessingServiceMock.Object,
                this.specificationObjectProcessingServiceMock.Object,
                this.documentProcessingServiceMock.Object,
                this.landingConfiguration,
                this.blobContainers,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            {
                CallBase = true
            };

            this.documentProcessingServiceMock
                .Setup(service => service.RemoveDocumentByFileNameAsync(
                    batchReadyFilePath,
                    blobContainers.Ingress))
                .Returns(ValueTask.CompletedTask);

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.Is(SameStreamAs(batchReadyStream)),
                    batchReadyFilePath,
                    blobContainers.Ingress))
                .Callback<Stream, string, string>((output, fileName, container) =>
                {
                    batchReadyStream.Position = 0;
                    batchReadyStream.CopyTo(actualStream);
                })
                .Returns(ValueTask.CompletedTask);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.MarkAsBatchCompleteAsync(ingestionTrackingId, true))
                    .Returns(ValueTask.CompletedTask);

            // when
            await ingressOrchestrationServiceMock.Object.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference,
                    storageIngestionTracking.SubscriberAgreementId.Value,
                    true),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(batchReadyFilePath, blobContainers.Ingress),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    batchReadyFilePath,
                    blobContainers.Ingress),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.MarkAsBatchCompleteAsync(ingestionTrackingId, true),
                    Times.Once);

            this.auditBrokerMock.Verify(service => service.LogInformationAsync(
                "BatchComplete",
                    $"{batchReadyFileName} generated for subscriber agreement " +
                        $"'{randomIngestionTracking.SubscriberAgreementId}' and " +
                            $"batch '{randomIngestionTracking.Batch}'",
                message,
                batchReadyFilePath,
                randomIngestionTracking.Batch),
                    Times.Once);

            this.loggingBrokerMock.Verify(service =>
                service.LogInformationAsync(message),
                    Times.Once);

            Assert.True(IsSameStream(expectedStream, actualStream));

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
