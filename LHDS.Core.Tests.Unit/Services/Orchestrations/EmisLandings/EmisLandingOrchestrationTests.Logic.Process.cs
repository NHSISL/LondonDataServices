// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessNewDocumentsAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = landingConfiguration.LandingSupplierId;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();

            List<Download> externalDownloads = randomDocuments.Select(document =>
                new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = document
                }).ToList();

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            string randomHash = GetRandomString(64);

            IQueryable<DataSetSpecification> randomDataSetSpecificationList =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomGuid);

            this.downloadProcessingServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                   .ReturnsAsync(externalDownloads);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            foreach (var downloadItem in externalDownloads)
            {
                this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());

                this.downloadProcessingServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(downloadItem))
                      .ReturnsAsync(downloadItem);

                this.hashBrokerMock.Setup(broker =>
                    broker.GenerateSha256Hash(downloadItem.Document.DocumentData))
                        .Returns(randomHash);

                var filename = downloadItem.Document.FileName.StartsWith('/')
                    ? downloadItem.Document.FileName
                    : "/" + downloadItem.Document.FileName;

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = randomGuid,
                        FileName = downloadItem.Document.FileName,
                        SupplierId = landingConfiguration.LandingSupplierId,
                        EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                        DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                            + $"/{randomDataSet.DataSetName}"
                            + $"/{randomDataSetSpecification.Id}"
                            + $"/{filename.Split('_')[3]}"
                            + $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                        Decrypted = false,
                        LastSeen = randomDateTime,
                        FileDeleted = false,
                        RecordCount = 0,
                        EncryptedFileSize = downloadItem.Document.DocumentData.Length,
                        EncryptedFileSha256Hash = randomHash,
                        DecryptedFileSize = 0,
                        DecryptedFileSha256Hash = string.Empty,
                        CreatedBy = "System",
                        CreatedDate = randomDateTime,
                        UpdatedBy = "System",
                        UpdatedDate = randomDateTime
                    };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newIngestionTracking))))
                        .ReturnsAsync(storageIngestionTracking);
            }

            // when
            await this.emisLandingOrchestrationService.ProcessAsync(subscriberCredential: inputSubscriberCredential);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload),
                    Times.Once);

            foreach (var downloadItem in externalDownloads)
            {
                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.RetrieveAllIngestionTrackings(),
                        Times.Exactly(2));

                this.hashBrokerMock.Verify(broker =>
                    broker.GenerateSha256Hash(downloadItem.Document.DocumentData),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDownloads.Count * 2));

                var filename = downloadItem.Document.FileName.StartsWith('/')
                    ? downloadItem.Document.FileName
                    : "/" + downloadItem.Document.FileName;

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = randomGuid,
                      FileName = downloadItem.Document.FileName,
                      SupplierId = landingConfiguration.LandingSupplierId,
                      EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}{filename}",

                      DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                            + $"/{randomDataSet.DataSetName}"
                            + $"/{randomDataSetSpecification.Id}"
                            + $"/{filename.Split('_')[3]}"
                            + $"{filename.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                      Decrypted = false,
                      LastSeen = randomDateTime,
                      FileDeleted = false,
                      RecordCount = 0,
                      EncryptedFileSize = downloadItem.Document.DocumentData.Length,
                      EncryptedFileSha256Hash = randomHash,
                      DecryptedFileSize = 0,
                      DecryptedFileSha256Hash = string.Empty,
                      CreatedBy = "System",
                      CreatedDate = randomDateTime,
                      UpdatedBy = "System",
                      UpdatedDate = randomDateTime
                  };

                IngestionTracking storageIngestionTracking = newIngestionTracking.DeepClone();

                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                        storageIngestionTracking))),
                            Times.Once);

                this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                    service.GetActiveDataSetSpecification(supplierId),
                        Times.Once);

                this.downloadProcessingServiceMock.Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(downloadItem),
                        Times.Once);

                Document newBlobDocument = new Document
                {
                    DocumentData = downloadItem.Document.DocumentData,
                    FileName = newIngestionTracking.EncryptedFileName
                };

                this.documentProcessingServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                        Times.Once);

                this.auditServiceMock.Verify(service =>
                    service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                        Times.Once);
            }

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessExistingDocumentsAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<Document> randomDocuments = CreateRandomDocuments();
            List<Document> externalDocuments = randomDocuments;

            List<Download> externalDownloads = externalDocuments.Select(document =>
                new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = document
                }).ToList();

            string randomHash = GetRandomString(64);

            List<IngestionTracking> externalIngestionTrackingsFound =
                CreateRandomIngestionTrackings(randomDateTime, randomDocuments);

            this.downloadProcessingServiceMock.Setup(service =>
               service.RetrieveListOfDocumentsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                   .ReturnsAsync(externalDownloads);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(It.IsAny<byte[]>()))
                    .Returns(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());
            }

            // when
            await this.emisLandingOrchestrationService.ProcessAsync(subscriberCredential: inputSubscriberCredential);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            foreach (var document in externalDocuments)
            {
                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.RetrieveAllIngestionTrackings(),
                        Times.Exactly(2));

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.AtLeastOnce);

                var maybeIngestionTracking = externalIngestionTrackingsFound
                    .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == document.FileName);

                maybeIngestionTracking.LastSeen = randomDateTime;

                this.ingestionTrackingProcessingServiceMock.Verify(broker =>
                    broker.ModifyIngestionTrackingAsync(maybeIngestionTracking),
                        Times.Once);
            }

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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

            List<Download> externalDownloads = externalDocuments.Select(document =>
                new Download
                {
                    SubscriberCredential = inputSubscriberCredential,
                    Document = document
                }).ToList();

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>
            {
                new IngestionTracking
                {
                    Id = Guid.NewGuid(),
                    SupplierId = this.landingConfiguration.LandingSupplierId,
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
               service.RetrieveListOfDocumentsToProcessAsync(inputDownload))
                   .ReturnsAsync(externalDownloads);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            // when
            await this.emisLandingOrchestrationService.ProcessAsync(subscriberCredential: inputSubscriberCredential);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(inputDownload),
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
        }
    }
}