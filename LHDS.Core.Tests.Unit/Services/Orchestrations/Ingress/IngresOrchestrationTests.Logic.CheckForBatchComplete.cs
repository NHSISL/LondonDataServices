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
            List<string> missingSpecificationObjectIds = new List<string> { dataSetSpecificationObjects.First() };

            string batchReadyFileName =
                $"{randomIngestionTracking.BatchReadyFolderPath}/BatchReady.txt";

            string batchIncompleteFileName =
                $"{randomIngestionTracking.BatchReadyFolderPath}/BatchNotReady.txt";

            string message =
                    $"Unable to generate '{batchReadyFileName}' for batch: {randomIngestionTracking.Batch}.  " +
                    Environment.NewLine +
                    $"We are missing {missingSpecificationObjectIds.Count}/{dataSetSpecificationObjects.Count} files.  " +
                    Environment.NewLine +
                    $"Missing specification object Id's: {string.Join(", ", missingSpecificationObjectIds)} " +
                    Environment.NewLine +
                    $"as defined by Dataset Specification Id: {randomIngestionTracking.DataSetSpecificationId}"; ;

            this.ingestionTrackingProcessingServiceMock
                .Setup(service => service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId))
                .ReturnsAsync(storageIngestionTracking);

            this.specificationObjectProcessingServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId))
                    .ReturnsAsync(dataSetSpecificationObjects);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference, true))
                    .ReturnsAsync(ingestionTrackingObjects);

            // when
            await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference, true),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    batchIncompleteFileName,
                    blobContainers.Ingress),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(
                    batchReadyFileName,
                    blobContainers.Ingress),
                        Times.Once);

            this.auditBrokerMock.Verify(service => service.LogInformationAsync(
                "BatchComplete",
                "Unable to generate BatchReady.txt",
                message,
                batchReadyFileName,
                randomIngestionTracking.Batch),
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

            string batchReadyFileName =
                $"{randomIngestionTracking.BatchReadyFolderPath}/BatchReady.txt";

            string batchIncompleteFileName =
                $"{randomIngestionTracking.BatchReadyFolderPath}/BatchNotReady.txt";

            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();
            Guid ingestionTrackingId = randomIngestionTracking.Id;
            string batchReference = randomIngestionTracking.Batch;
            Guid datasetSpecificationId = randomIngestionTracking.DataSetSpecificationId;
            List<string> randomObjects = GetRandomStringList();
            List<string> ingestionTrackingObjects = randomObjects.DeepClone();
            List<string> dataSetSpecificationObjects = randomObjects.DeepClone();

            string message =
                $"All specification object files present for batch '{randomIngestionTracking.Batch}' " +
                $"as defined in Dataset Specification Id: '{randomIngestionTracking.DataSetSpecificationId}'." +
                Environment.NewLine +
                $"Generate batch complete file: '{batchReadyFileName}'";

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
                service.RetrieveObjectsInBatchByBatchReference(batchReference, true))
                    .ReturnsAsync(ingestionTrackingObjects);

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.Is(SameStreamAs(batchReadyStream)),
                    batchReadyFileName,
                    blobContainers.Ingress))
                .Callback<Stream, string, string>((output, fileName, container) =>
                {
                    batchReadyStream.Position = 0;
                    batchReadyStream.CopyTo(actualStream);
                })
                .Returns(ValueTask.CompletedTask);

            // when
            await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(ingestionTrackingId),
                    Times.Once);

            this.specificationObjectProcessingServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectsByDataSetSpecificationIdAsync(datasetSpecificationId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveObjectsInBatchByBatchReference(batchReference, true),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    batchReadyFileName,
                    blobContainers.Ingress),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(
                    batchIncompleteFileName,
                    blobContainers.Ingress),
                        Times.Once);

            this.auditBrokerMock.Verify(service => service.LogInformationAsync(
                "BatchComplete",
                "BatchReady.txt generated",
                message,
                batchReadyFileName,
                randomIngestionTracking.Batch),
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
