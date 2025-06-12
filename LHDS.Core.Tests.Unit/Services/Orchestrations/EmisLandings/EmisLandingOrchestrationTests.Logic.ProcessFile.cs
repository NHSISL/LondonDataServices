// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessNewFileAndUpdateLastSeenAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = randomGuid;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            int count = GetRandomNumber();

            string randomFileName =
                GetRandomFilePaths(count: 1, subscriberAgreementId: inputSubscriberCredential.Id).First();

            string inputFileName = randomFileName;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string tempFileName = GetRandomString();
            string randomHash = GetRandomString(64);
            string container = blobContainers.EmisLanding;

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(new List<IngestionTracking>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomGuid);

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(), It.IsAny<string>()))
                        .Returns(ValueTask.CompletedTask);

            DataSet randomDataSet = CreateRandomDataSet(supplierId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(dataSet: randomDataSet);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecification));

            var filename = inputFileName.StartsWith('/')
                ? inputFileName
                : "/" + inputFileName;

            string fileWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            string[] segments = fileWithoutExtension.Split('_');
            string batch = $"{segments[0]}_{segments[1]}";
            string objectName = $"{segments[2]}_{segments[3]}";

            (string encryptedFileName, string decryptedFileName, string baseFolder) = GetFileNames(
                inputSubscriberCredential,
                randomDataSet,
                randomDataSetSpecification,
                filename);

            string expectedFileName = decryptedFileName;
            string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
            sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");

            IngestionTracking newIngestionTrackingItem =
                new IngestionTracking
                {
                    Id = randomGuid,
                    SupplierId = supplierId,
                    FileName = filename,
                    SourceFolderPath = sourceFolderPath,
                    BatchReadyFolderPath = baseFolder,
                    Batch = batch,
                    IsBatchComplete = false,
                    ObjectName = objectName,
                    DataSetSpecificationId = randomDataSetSpecification.Id,
                    EncryptedFileName = encryptedFileName,
                    DecryptedFileName = decryptedFileName,
                    Decrypted = false,
                    LastSeen = randomDateTime,
                    FileDeleted = false,
                    RetryCount = 0,
                    LastAttempt = randomDateTime,
                    EncryptedFileSize = 0,
                    EncryptedFileSha256Hash = string.Empty,
                    DecryptedFileSize = 0,
                    DecryptedFileSha256Hash = string.Empty,
                    IsDownloaded = false,
                    SubscriberAgreementId = inputSubscriberCredential.Id
                };

            IngestionTracking storageIngestionTracking = newIngestionTrackingItem.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTrackingItem))))
                    .ReturnsAsync(storageIngestionTracking);

            string batchCompleteFileName =
                $"{storageIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            this.documentProcessingServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(batchCompleteFileName, this.blobContainers.Ingress))
                    .Returns(ValueTask.CompletedTask);

            IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
            modifiedIngestionTracking.RetryCount += 1;
            modifiedIngestionTracking.IsDownloaded = false;
            modifiedIngestionTracking.IsBatchComplete = false;
            modifiedIngestionTracking.FileDeleted = false;
            modifiedIngestionTracking.EncryptedFileSize = 0;
            modifiedIngestionTracking.EncryptedFileSha256Hash = string.Empty;
            IngestionTracking downloadingIngestionTracking = modifiedIngestionTracking.DeepClone();
            var mockSequence = new MockSequence();

            this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
                service.ModifyIngestionTrackingAsync(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))))
                        .ReturnsAsync(downloadingIngestionTracking);

            this.fileBrokerMock
                .Setup(broker => broker.GetTempFileName())
                    .ReturnsAsync(tempFileName);

            Download inputFileDownload = new Download
            {
                Document = new Document
                {
                    FileName = downloadingIngestionTracking.FileName,
                    DocumentData = new MemoryStream()
                },
                SubscriberCredential = inputDownload.SubscriberCredential
            };

            Download storageFileDownload = inputFileDownload.DeepClone();
            Stream downloadedContent = new MemoryStream(Encoding.UTF8.GetBytes(filename));

            this.downloadProcessingServiceMock
                .Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))))
                .Callback<Download>(download =>
                {
                    downloadedContent.Position = 0;
                    downloadedContent.CopyTo(download.Document.DocumentData);
                    downloadedContent.Position = 0;
                    downloadedContent.CopyTo(inputFileDownload.Document.DocumentData);
                })
                .Returns(ValueTask.CompletedTask);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()))
                    .ReturnsAsync(randomHash);

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    downloadingIngestionTracking.EncryptedFileName,
                    container))
                .Returns(ValueTask.CompletedTask);

            IngestionTracking downloadedIngestionTracking = downloadingIngestionTracking.DeepClone();
            downloadedIngestionTracking.IsDownloaded = true;
            downloadedIngestionTracking.Decrypted = false;
            downloadedIngestionTracking.IsProcessing = false;
            downloadedIngestionTracking.RetryCount = 0;
            downloadedIngestionTracking.FileDeleted = false;
            downloadedIngestionTracking.LastSeen = randomDateTime;
            downloadedIngestionTracking.EncryptedFileSize = downloadedContent.Length;
            downloadedIngestionTracking.EncryptedFileSha256Hash = randomHash;
            IngestionTracking uploadedIngestionTracking = downloadedIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(downloadedIngestionTracking))))
                    .ReturnsAsync(uploadedIngestionTracking);

            // when
            string actualFileName = await emisLandingOrchestrationServiceMock.Object.ProcessFileAsync(
                subscriberCredential: inputSubscriberCredential, supplierId, inputFileName);

            // then
            actualFileName.Should().BeEquivalentTo(expectedFileName);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(supplierId),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTrackingItem))),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(
                        SameIngestionTrackingAs(storageIngestionTracking)),
                        $"New file found '{storageIngestionTracking.FileName}',  " +
                            $"created item with Id: {storageIngestionTracking.Id}"),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Processing file '{modifiedIngestionTracking.FileName}' " +
                        $"associated with Id: {modifiedIngestionTracking.Id}." + Environment.NewLine +
                            $"Downloading: {modifiedIngestionTracking.FileName} " + Environment.NewLine +
                                $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                    Times.Once);

            string batchReadyFileName =
                $"{storageIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(
                        SameIngestionTrackingAs(modifiedIngestionTracking)),
                        $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                            $"ready status for batch: {storageIngestionTracking.Batch}."),
                                Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(batchCompleteFileName, this.blobContainers.Ingress),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(storageIngestionTracking)),
                    $"Downloading {storageIngestionTracking.FileName};  " +
                        $"RetryCount: {storageIngestionTracking.RetryCount}"),
                            Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))),
                        Times.Once);

            this.fileBrokerMock
                .Verify(broker => broker.GetTempFileName(),
                    Times.Once);

            this.downloadProcessingServiceMock
                .Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))),
                        Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()),
                    Times.Once);

            this.documentProcessingServiceMock
                .Verify(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    downloadingIngestionTracking.EncryptedFileName,
                    container),
                        Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Downloaded file '{downloadingIngestionTracking.FileName}' " +
                        $"and successfully uploaded to blob storage " +
                            $"'{downloadingIngestionTracking.EncryptedFileName}'"),
                                Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(downloadingIngestionTracking))),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(uploadedIngestionTracking)),
                    $"Updated ingestion tracking info to " +
                        $"reflect successful processing of {uploadedIngestionTracking.FileName}"),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessExistingFilesNotDownloadedAndWhereRetryNotExceededAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = randomGuid;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            int count = GetRandomNumber();

            string randomFileName =
                GetRandomFilePaths(count: 1, subscriberAgreementId: inputSubscriberCredential.Id).First();

            string inputFileName = randomFileName;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string tempFileName = GetRandomString();
            string randomHash = GetRandomString(64);
            string container = blobContainers.EmisLanding;

            IngestionTracking storageIngestionTracking =
                CreateRandomIngestionTracking(
                    dateTimeOffset: randomDateTime,
                    fileName: inputFileName,
                    isDownloaded: false,
                    retryCount: 1);

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(new List<IngestionTracking> { storageIngestionTracking }.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomGuid);

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.LogAudit(
                    It.IsAny<IngestionTracking>(), It.IsAny<string>()))
                        .Returns(ValueTask.CompletedTask);

            DataSet randomDataSet = CreateRandomDataSet(supplierId);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(dataSet: randomDataSet);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecification));

            string batchCompleteFileName =
                $"{storageIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            this.documentProcessingServiceMock.Setup(service =>
                service.RemoveDocumentByFileNameAsync(batchCompleteFileName, this.blobContainers.Ingress))
                    .Returns(ValueTask.CompletedTask);

            IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
            modifiedIngestionTracking.RetryCount += 1;
            modifiedIngestionTracking.IsDownloaded = false;
            modifiedIngestionTracking.IsBatchComplete = false;
            modifiedIngestionTracking.FileDeleted = false;
            modifiedIngestionTracking.EncryptedFileSize = 0;
            modifiedIngestionTracking.EncryptedFileSha256Hash = string.Empty;
            IngestionTracking downloadingIngestionTracking = modifiedIngestionTracking.DeepClone();
            var mockSequence = new MockSequence();

            this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
                service.ModifyIngestionTrackingAsync(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))))
                        .ReturnsAsync(downloadingIngestionTracking);

            this.fileBrokerMock
                .Setup(broker => broker.GetTempFileName())
                    .ReturnsAsync(tempFileName);

            Download inputFileDownload = new Download
            {
                Document = new Document
                {
                    FileName = downloadingIngestionTracking.FileName,
                    DocumentData = new MemoryStream()
                },
                SubscriberCredential = inputDownload.SubscriberCredential
            };

            Download storageFileDownload = inputFileDownload.DeepClone();
            Stream downloadedContent = new MemoryStream(Encoding.UTF8.GetBytes(storageIngestionTracking.FileName));

            this.downloadProcessingServiceMock
                .Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))))
                .Callback<Download>(download =>
                {
                    downloadedContent.Position = 0;
                    downloadedContent.CopyTo(download.Document.DocumentData);
                    downloadedContent.Position = 0;
                    downloadedContent.CopyTo(inputFileDownload.Document.DocumentData);
                })
                .Returns(ValueTask.CompletedTask);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()))
                    .ReturnsAsync(randomHash);

            this.documentProcessingServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    downloadingIngestionTracking.EncryptedFileName,
                    container))
                .Returns(ValueTask.CompletedTask);

            IngestionTracking downloadedIngestionTracking = downloadingIngestionTracking.DeepClone();
            downloadedIngestionTracking.IsDownloaded = true;
            downloadedIngestionTracking.Decrypted = false;
            downloadedIngestionTracking.IsProcessing = false;
            downloadedIngestionTracking.RetryCount = 0;
            downloadedIngestionTracking.FileDeleted = false;
            downloadedIngestionTracking.LastSeen = randomDateTime;
            downloadedIngestionTracking.EncryptedFileSize = downloadedContent.Length;
            downloadedIngestionTracking.EncryptedFileSha256Hash = randomHash;
            IngestionTracking uploadedIngestionTracking = downloadedIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(downloadedIngestionTracking))))
                    .ReturnsAsync(uploadedIngestionTracking);

            string expectedFileName = storageIngestionTracking.DecryptedFileName;

            // when
            string actualFileName = await emisLandingOrchestrationServiceMock.Object.ProcessFileAsync(
                subscriberCredential: inputSubscriberCredential, supplierId, inputFileName);

            // then
            actualFileName.Should().BeEquivalentTo(expectedFileName);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Processing file '{modifiedIngestionTracking.FileName}' " +
                        $"associated with Id: {modifiedIngestionTracking.Id}." + Environment.NewLine +
                            $"Downloading: {modifiedIngestionTracking.FileName} " + Environment.NewLine +
                                $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                    Times.Once);

            string batchReadyFileName =
                $"{storageIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}"
                    .Replace("\\", "/");

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(
                        SameIngestionTrackingAs(modifiedIngestionTracking)),
                        $"Removing batch ready file '{batchReadyFileName}' as this file will invalidate the " +
                            $"ready status for batch: {storageIngestionTracking.Batch}."),
                                Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(batchCompleteFileName, this.blobContainers.Ingress),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(
                        SameIngestionTrackingAs(modifiedIngestionTracking)),
                        $"Downloading {modifiedIngestionTracking.FileName};  " +
                            $"RetryCount: {modifiedIngestionTracking.RetryCount}"),
                                Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))),
                        Times.Once);

            this.fileBrokerMock
                .Verify(broker => broker.GetTempFileName(),
                    Times.Once);

            this.downloadProcessingServiceMock
                .Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))),
                        Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()),
                    Times.Once);

            this.documentProcessingServiceMock
                .Verify(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    downloadingIngestionTracking.EncryptedFileName,
                    container),
                        Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(modifiedIngestionTracking)),
                    $"Downloaded file '{downloadingIngestionTracking.FileName}' " +
                        $"and successfully uploaded to blob storage " +
                            $"'{downloadingIngestionTracking.EncryptedFileName}'"),
                                Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(downloadingIngestionTracking))),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.LogAudit(
                    It.Is(SameIngestionTrackingAs(uploadedIngestionTracking)),
                    $"Updated ingestion tracking info to " +
                        $"reflect successful processing of {uploadedIngestionTracking.FileName}"),
                            Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 4)]
        [InlineData(true, 4)]
        public async Task ShouldOnlyUpdateLastSeenDateForFilesAlreadyProcessedOrWhereRetryCountExceededAsync(
            bool isDownloaded,
            int retryCount)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = randomGuid;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            int count = GetRandomNumber();

            string randomFileName =
                GetRandomFilePaths(count: 1, subscriberAgreementId: inputSubscriberCredential.Id).First();

            string inputFileName = randomFileName;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            string tempFileName = GetRandomString();
            string randomHash = GetRandomString(64);
            string container = blobContainers.EmisLanding;

            IngestionTracking storageIngestionTracking =
                CreateRandomIngestionTracking(
                    dateTimeOffset: randomDateTime,
                    fileName: inputFileName,
                    isDownloaded: isDownloaded,
                    retryCount: retryCount);

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(new List<IngestionTracking> { storageIngestionTracking }.AsQueryable());

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(storageIngestionTracking.Id))
                    .ReturnsAsync(storageIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTime);

            IngestionTracking lastSeenIngestionTracking = storageIngestionTracking.DeepClone();
            lastSeenIngestionTracking.LastSeen = randomDateTime;

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(lastSeenIngestionTracking))))
                    .ReturnsAsync(lastSeenIngestionTracking);

            string expectedFileName = string.Empty;

            // when
            string actualFileName = await emisLandingOrchestrationServiceMock.Object.ProcessFileAsync(
                subscriberCredential: inputSubscriberCredential, supplierId, inputFileName);

            // then
            actualFileName.Should().BeEquivalentTo(expectedFileName);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(storageIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(
                    It.Is(SameIngestionTrackingAs(lastSeenIngestionTracking))),
                        Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}