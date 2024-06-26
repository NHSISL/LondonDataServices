// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact(Skip = "Convert to stream")]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            //// given
            //SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            //SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            //Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            //Guid randomGuid = Guid.NewGuid();
            //Guid supplierId = landingConfiguration.LandingSupplierId;
            //DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            //List<Document> randomDocuments = CreateRandomDocuments();
            //List<string> externalDownloadList = randomDocuments.Select(document => document.FileName).ToList();
            //List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();
            //DataSet randomDataSet = CreateRandomDataSet(supplierId);
            //string randomHash = GetRandomString(64);
            //string container = blobContainers.EmisLanding;
            //string randomFilePath = GetRandomString();

            //IQueryable<DataSetSpecification> randomDataSetSpecificationList =
            //    CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            //DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();

            //this.downloadProcessingServiceMock.Setup(service =>
            //   service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
            //       .ReturnsAsync(externalDownloadList);

            //this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //    service.RetrieveAllIngestionTrackings())
            //        .Returns(new List<IngestionTracking>().AsQueryable());

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTimeOffset())
            //        .Returns(randomDateTime);

            //this.identifierBrokerMock.Setup(broker =>
            //    broker.GetIdentifier())
            //        .Returns(randomGuid);

            //this.fileBrokerMock.Setup(broker =>
            //    broker.GetTempFileName())
            //        .ReturnsAsync(randomFilePath);

            //this.dataSetSpecificationProcessingServiceMock.Setup(service =>
            //    service.GetActiveDataSetSpecification(supplierId))
            //        .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            //foreach (var externalFileName in externalDownloadList)
            //{
            //    var filename = externalFileName.StartsWith('/')
            //        ? externalFileName
            //        : "/" + externalFileName;

            //    (string encryptedFileName, string decryptedFileName) = GetFileNames(
            //        inputSubscriberCredential,
            //        randomDataSet,
            //        randomDataSetSpecification,
            //        filename);

            //    string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
            //    sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");

            //    IngestionTracking newIngestionTracking =
            //        new IngestionTracking
            //        {
            //            Id = randomGuid,
            //            FileName = filename,
            //            SourceFolderPath = sourceFolderPath,
            //            SupplierId = landingConfiguration.LandingSupplierId,
            //            EncryptedFileName = encryptedFileName,
            //            DecryptedFileName = decryptedFileName,
            //            Decrypted = false,
            //            LastSeen = randomDateTime,
            //            FileDeleted = false,
            //            RecordCount = 0,
            //            RetryCount = 0,
            //            LastAttempt = randomDateTime,
            //            EncryptedFileSize = 0,
            //            EncryptedFileSha256Hash = string.Empty,
            //            DecryptedFileSize = 0,
            //            DecryptedFileSha256Hash = string.Empty,
            //            IsDownloaded = false,
            //            CreatedBy = "System",
            //            CreatedDate = randomDateTime,
            //            UpdatedBy = "System",
            //            UpdatedDate = randomDateTime
            //        };

            //    IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

            //    this.ingestionTrackingProcessingServiceMock.Setup(service =>
            //        service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))))
            //            .ReturnsAsync(storageIngestionTracking);

            //    //=======================

            //    IngestionTracking retryDownloadIngestionTracking = storageIngestionTracking.DeepClone();
            //    retryDownloadIngestionTracking.RetryCount += 1;
            //    retryDownloadIngestionTracking.IsDownloaded = false;
            //    retryDownloadIngestionTracking.FileDeleted = false;
            //    retryDownloadIngestionTracking.EncryptedFileSize = 0;
            //    retryDownloadIngestionTracking.EncryptedFileSha256Hash = string.Empty;
            //    retryDownloadIngestionTracking.LastSeen = randomDateTime;
            //    retryDownloadIngestionTracking.UpdatedDate = randomDateTime;

            //    IngestionTracking downloadingIngestionTracking = retryDownloadIngestionTracking.DeepClone();
            //    var mockSequence = new MockSequence();

            //    this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
            //        service.ModifyIngestionTrackingAsync(
            //            It.Is(SameIngestionTrackingAs(retryDownloadIngestionTracking))))
            //                .ReturnsAsync(downloadingIngestionTracking);

            //    //=======================
            //    Download inputFileDownload = new Download
            //    {
            //        Document = new Document
            //        {
            //            FileName = externalFileName,
            //            DocumentData = new MemoryStream()
            //        },
            //        SubscriberCredential = inputDownload.SubscriberCredential
            //    };

            //    Download storageFileDownload = inputFileDownload.DeepClone();

            //    Stream downloadedContent = new MemoryStream(
            //        Encoding.ASCII.GetBytes(externalFileName));

            //    this.downloadProcessingServiceMock
            //        .Setup(service =>
            //            service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))))
            //        .Callback<Download>(download =>
            //            {
            //                downloadedContent.Position = 0;
            //                downloadedContent.CopyTo(download.Document.DocumentData);
            //            })
            //        .Returns(ValueTask.CompletedTask);

            //    this.hashBrokerMock.Setup(broker =>
            //        broker.GenerateSha256Hash(It.Is(SameStreamAs(downloadedContent))))
            //            .Returns(randomHash);

            //    //=======================

            //    IngestionTracking modifiedIngestionTracking = downloadingIngestionTracking.DeepClone();
            //    modifiedIngestionTracking.IsDownloaded = true;
            //    modifiedIngestionTracking.RetryCount = 0;
            //    modifiedIngestionTracking.FileDeleted = false;
            //    modifiedIngestionTracking.EncryptedFileSize = downloadedContent.Length;
            //    modifiedIngestionTracking.EncryptedFileSha256Hash = randomHash;
            //    modifiedIngestionTracking.LastSeen = randomDateTime;
            //    modifiedIngestionTracking.UpdatedDate = randomDateTime;
            //    IngestionTracking downloadedIngestionTracking = modifiedIngestionTracking.DeepClone();

            //    this.ingestionTrackingProcessingServiceMock.InSequence(mockSequence).Setup(service =>
            //        service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))))
            //            .ReturnsAsync(downloadedIngestionTracking);
            //}

            //// when
            //await this.emisLandingOrchestrationService.ProcessAsync(
            //    subscriberCredential: inputSubscriberCredential, supplierId);

            //// then
            //this.downloadProcessingServiceMock.Verify(service =>
            //    service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
            //        Times.Once);

            //this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //    service.RetrieveAllIngestionTrackings(),
            //        Times.Exactly(externalDownloadList.Count + 1));

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Exactly(5 * externalDownloadList.Count));

            //this.dataSetSpecificationProcessingServiceMock.Verify(service =>
            //    service.GetActiveDataSetSpecification(supplierId),
            //        Times.Once);

            //this.fileBrokerMock.Verify(broker =>
            //    broker.GetTempFileName(),
            //        Times.Once);

            //this.fileBrokerMock.Verify(broker =>
            //    broker.DeleteFileAsync(randomFilePath),
            //        Times.Once);

            //this.auditServiceMock.Verify(service =>
            //    service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
            //        Times.Exactly(3));

            //foreach (var externalFileName in externalDownloadList)
            //{
            //    Download inputFileDownload = new Download
            //    {
            //        Document = new Document { FileName = externalFileName },
            //        SubscriberCredential = inputDownload.SubscriberCredential
            //    };

            //    Download storageFileDownload = inputFileDownload.DeepClone();

            //    Stream downloadedContent = new MemoryStream(
            //        Encoding.ASCII.GetBytes(externalFileName));

            //    this.downloadProcessingServiceMock.Verify(service =>
            //        service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))),
            //            Times.Once);

            //    this.hashBrokerMock.Verify(broker =>
            //        broker.GenerateSha256Hash(downloadedContent),
            //            Times.Once);

            //    var filename = externalFileName.StartsWith('/')
            //        ? externalFileName
            //        : "/" + externalFileName;

            //    (string encryptedFileName, string decryptedFileName) = GetFileNames(
            //        inputSubscriberCredential,
            //        randomDataSet,
            //        randomDataSetSpecification,
            //        filename);

            //    string sourceFolderPath = Path.GetDirectoryName(filename) ?? string.Empty;
            //    sourceFolderPath = sourceFolderPath.Replace("\\", "/").Replace("\\", "/");

            //    IngestionTracking newIngestionTracking =
            //        new IngestionTracking
            //        {
            //            Id = randomGuid,
            //            FileName = filename,
            //            SourceFolderPath = sourceFolderPath,
            //            SupplierId = landingConfiguration.LandingSupplierId,
            //            EncryptedFileName = encryptedFileName,
            //            DecryptedFileName = decryptedFileName,
            //            Decrypted = false,
            //            LastSeen = randomDateTime,
            //            FileDeleted = false,
            //            RecordCount = 0,
            //            RetryCount = 0,
            //            LastAttempt = randomDateTime,
            //            EncryptedFileSize = 0,
            //            EncryptedFileSha256Hash = string.Empty,
            //            DecryptedFileSize = 0,
            //            DecryptedFileSha256Hash = string.Empty,
            //            IsDownloaded = false,
            //            CreatedBy = "System",
            //            CreatedDate = randomDateTime,
            //            UpdatedBy = "System",
            //            UpdatedDate = randomDateTime
            //        };

            //    IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

            //    this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //        service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))),
            //            Times.Once);

            //    IngestionTracking retryDownloadIngestionTracking = storageIngestionTracking.DeepClone();
            //    retryDownloadIngestionTracking.RetryCount += 1;
            //    retryDownloadIngestionTracking.IsDownloaded = false;
            //    retryDownloadIngestionTracking.FileDeleted = false;
            //    retryDownloadIngestionTracking.EncryptedFileSize = 0;
            //    retryDownloadIngestionTracking.EncryptedFileSha256Hash = string.Empty;
            //    retryDownloadIngestionTracking.LastSeen = randomDateTime;
            //    retryDownloadIngestionTracking.UpdatedDate = randomDateTime;
            //    IngestionTracking downloadingIngestionTracking = retryDownloadIngestionTracking.DeepClone();

            //    this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //        service.ModifyIngestionTrackingAsync(
            //            It.Is(SameIngestionTrackingAs(retryDownloadIngestionTracking))),
            //                Times.Once);

            //    IngestionTracking modifiedIngestionTracking = downloadingIngestionTracking.DeepClone();
            //    modifiedIngestionTracking.IsDownloaded = true;
            //    modifiedIngestionTracking.RetryCount = 0;
            //    modifiedIngestionTracking.FileDeleted = false;
            //    modifiedIngestionTracking.EncryptedFileSize = storageFileDownload.Document.DocumentData.Length;
            //    modifiedIngestionTracking.EncryptedFileSha256Hash = randomHash;
            //    modifiedIngestionTracking.LastSeen = randomDateTime;
            //    modifiedIngestionTracking.UpdatedDate = randomDateTime;
            //    IngestionTracking downloadedIngestionTracking = modifiedIngestionTracking.DeepClone();

            //    Document newBlobDocument = new Document
            //    {
            //        DocumentData = storageFileDownload?.Document?.DocumentData ?? Array.Empty<byte>(),
            //        FileName = modifiedIngestionTracking.EncryptedFileName,
            //        SHA256Hash = randomHash
            //    };

            //    this.documentProcessingServiceMock.Verify(service =>
            //        service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), container),
            //            Times.Once);

            //    this.ingestionTrackingProcessingServiceMock.Verify(service =>
            //        service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(modifiedIngestionTracking))),
            //            Times.Once);
            //}

            //this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            //this.hashBrokerMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            //this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            //this.documentProcessingServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
            //this.auditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 4)]
        [InlineData(true, 4)]
        public async Task ShouldNotProcessDownloadedDocumentsOrWhenRetryCountReachedAsync(
            bool isDownloaded,
            int retryCount)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<string> randomFileNames = GetRandomStrings();
            List<string> externalDownloadList = randomFileNames;
            string randomHash = GetRandomString(64);
            Guid inputSupplierId = landingConfiguration.LandingSupplierId;

            List<IngestionTracking> externalIngestionTrackingsFound =
                CreateRandomIngestionTrackings(
                    dateTimeOffset: randomDateTime,
                    fileNames: externalDownloadList,
                    isDownloaded,
                    retryCount);

            List<IngestionTracking> ftpFileList = new List<IngestionTracking>();
            ftpFileList.AddRange(externalIngestionTrackingsFound);

            this.downloadProcessingServiceMock.Setup(service =>
               service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                   .ReturnsAsync(externalDownloadList);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(ftpFileList.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            await this.emisLandingOrchestrationService.ProcessAsync(
                subscriberCredential: inputSubscriberCredential,
                supplierId: inputSupplierId);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Exactly(randomFileNames.Count + 1));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(externalDownloadList.Count));

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessDocumentsAndMarkUnavailableFilesAsDeletedAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            Guid randomGuid = Guid.NewGuid();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> externalDocuments = new List<Document>();
            List<string> externalDownloadList = externalDocuments.Select(document => document.FileName).ToList();
            Guid inputSupplierId = landingConfiguration.LandingSupplierId;

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>
            {
                new IngestionTracking
                {
                    Id = Guid.NewGuid(),
                    SupplierId = inputSupplierId,
                    FileName = "test.txt",
                    EncryptedFileName = "/encrypted/test.txt",
                    DecryptedFileName = "/decrypted/test.txt",
                    Decrypted = true,
                    LastSeen = randomDateTime.AddMinutes(-20),
                    FileDeleted = false,
                }
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomGuid);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                    .ReturnsAsync(externalDownloadList);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            // when
            await this.emisLandingOrchestrationService
                .ProcessAsync(subscriberCredential: inputSubscriberCredential, supplierId: inputSupplierId);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            DateTimeOffset expireTime = randomDateTime.AddMinutes(-15);

            List<IngestionTracking> expiredIngestionTrackings = externalIngestionTrackingsFound
                .Where(ingestionTracking => ingestionTracking.LastSeen <= expireTime).ToList();

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            foreach (var ingestionTracking in expiredIngestionTrackings)
            {
                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(expiredIngestionTrackings.Count + 1));

                ingestionTracking.FileDeleted = true;
                ingestionTracking.UpdatedDate = randomDateTime;

                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        ingestionTracking))),
                            Times.Once);
            }

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }

        private (string encryptedFileName, string decryptedFileName) GetFileNames(
            SubscriberCredential inputSubscriberCredential,
            DataSet randomDataSet,
            DataSetSpecification randomDataSetSpecification,
            string externalFileName)
        {
            var filename = externalFileName.StartsWith('/')
                ? externalFileName
                : "/" + externalFileName;

            string[] splitFileName = filename.Split('/');
            string newFileName = "";

            if (splitFileName.Length < 6)
            {
                throw new InvalidArgumentsDocumentProcessingException(filename);
            }
            else
            {
                newFileName = $"{inputSubscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
            }

            string decryptedFileName = $"/{landingConfiguration.DecryptedFolder}"
                        + $"/{randomDataSet.DataSetName}"
                        + $"/{randomDataSetSpecification.Id}"
                        + $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}"
                        + $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            string encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newFileName}";

            return (encryptedFileName, decryptedFileName);
        }
    }
}