// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.Documents.Exceptions;
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
            List<string> externalDownloadList = randomDocuments.Select(document => document.FileName).ToList();
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
               service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                   .ReturnsAsync(externalDownloadList);

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            foreach (var externalFileName in externalDownloadList)
            {
                Download inputFileDownload = new Download
                {
                    Document = new Document { FileName = externalFileName },
                    SubscriberCredential = inputDownload.SubscriberCredential
                };

                Download storageFileDownload = inputFileDownload.DeepClone();

                storageFileDownload.Document.DocumentData =
                    Encoding.ASCII.GetBytes(storageFileDownload.Document.FileName);

                this.ingestionTrackingProcessingServiceMock.Setup(service =>
                    service.RetrieveAllIngestionTrackings())
                        .Returns(externalIngestionTrackingsFound.AsQueryable());

                this.downloadProcessingServiceMock.Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputFileDownload))))
                        .ReturnsAsync(storageFileDownload);

                this.hashBrokerMock.Setup(broker =>
                    broker.GenerateSha256Hash(storageFileDownload.Document.DocumentData))
                        .Returns(randomHash);

                var filename = externalFileName.StartsWith('/')
                    ? externalFileName
                    : "/" + externalFileName;

                string[] splitFileName = filename.Split('/');
                string newFileName = "";

                if (splitFileName.Length < 6)
                {
                    throw new InvalidDocumentProcessingFileNameException(filename);
                }
                else
                {
                    newFileName = $"{inputSubscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
                }

                IngestionTracking newIngestionTracking =
                    new IngestionTracking
                    {
                        Id = randomGuid,
                        FileName = externalFileName,
                        SupplierId = landingConfiguration.LandingSupplierId,
                        EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{filename}",

                        DecryptedFileName =
                            $"/{landingConfiguration.DecryptedFolder}"
                            + $"/{randomDataSet.DataSetName}"
                            + $"/{randomDataSetSpecification.Id}"
                            + $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}"
                            + $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                        Decrypted = false,
                        LastSeen = randomDateTime,
                        FileDeleted = false,
                        RecordCount = 0,
                        EncryptedFileSize = storageFileDownload.Document.DocumentData.Length,
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
            await this.emisLandingOrchestrationService.ProcessAsync(
                subscriberCredential: inputSubscriberCredential, supplierId);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            foreach (var externalFileName in externalDownloadList)
            {
                Download inputNewFileDownload = new Download
                {
                    Document = new Document { FileName = externalFileName },
                    SubscriberCredential = inputDownload.SubscriberCredential
                };

                Download storageNewFileDownload = inputNewFileDownload.DeepClone();

                storageNewFileDownload.Document.DocumentData =
                    Encoding.ASCII.GetBytes(storageNewFileDownload.Document.FileName);

                this.ingestionTrackingProcessingServiceMock.Verify(service =>
                    service.RetrieveAllIngestionTrackings(),
                        Times.Exactly(2));

                this.downloadProcessingServiceMock.Verify(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputNewFileDownload))),
                        Times.Once);

                this.hashBrokerMock.Verify(broker =>
                    broker.GenerateSha256Hash(storageNewFileDownload.Document.DocumentData),
                        Times.Once);

                this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTimeOffset(),
                        Times.Exactly(externalDownloadList.Count * 2));

                var filename = externalFileName.StartsWith('/')
                    ? externalFileName
                    : "/" + externalFileName;

                string[] splitFileName = filename.Split('/');
                string newFileName = "";

                if (splitFileName.Length < 6)
                {
                    throw new InvalidDocumentProcessingFileNameException(filename);
                }
                else
                {
                    newFileName = $"{inputSubscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
                }

                IngestionTracking newIngestionTracking =
                  new IngestionTracking
                  {
                      Id = randomGuid,
                      FileName = externalFileName,
                      SupplierId = landingConfiguration.LandingSupplierId,
                      EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newFileName}",

                      DecryptedFileName =
                        $"/{landingConfiguration.DecryptedFolder}"
                        + $"/{randomDataSet.DataSetName}"
                        + $"/{randomDataSetSpecification.Id}"
                        + $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}"
                        + $"/{newFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

                      Decrypted = false,
                      LastSeen = randomDateTime,
                      FileDeleted = false,
                      RecordCount = 0,
                      EncryptedFileSize = storageNewFileDownload.Document.DocumentData.Length,
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

                Document newBlobDocument = new Document
                {
                    DocumentData = storageNewFileDownload.Document.DocumentData,
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
            List<string> externalDownloadList = randomDocuments.Select(document => document.FileName).ToList();
            string randomHash = GetRandomString(64);
            Guid inputSupplierId = landingConfiguration.LandingSupplierId;

            List<IngestionTracking> externalIngestionTrackingsFound =
                CreateRandomIngestionTrackings(dateTimeOffset: randomDateTime, fileNames: externalDownloadList);

            this.downloadProcessingServiceMock.Setup(service =>
               service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                   .ReturnsAsync(externalDownloadList);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(It.IsAny<byte[]>()))
                    .Returns(randomHash);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            // when
            await this.emisLandingOrchestrationService.ProcessAsync(
                subscriberCredential: inputSubscriberCredential,
                supplierId: inputSupplierId);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
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
        }
    }
}