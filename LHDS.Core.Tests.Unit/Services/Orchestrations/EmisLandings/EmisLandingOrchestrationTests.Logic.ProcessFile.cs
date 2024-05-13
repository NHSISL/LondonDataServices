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
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessExistingNamedDocumentAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            IngestionTracking externalIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            Guid inputSupplierId = externalIngestionTracking.SupplierId;

            List<IngestionTracking> externalIngestionTrackingsFound =
                new List<IngestionTracking> { externalIngestionTracking };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = externalIngestionTracking.FileName }
            };

            Download storageDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = externalDocument
            };

            this.downloadProcessingServiceMock.Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
                        .ReturnsAsync(storageDownload);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            IngestionTracking updatedIngestionTracking = externalIngestionTracking.DeepClone();
            updatedIngestionTracking.LastSeen = randomDateTime;
            updatedIngestionTracking.EncryptedFileSize = externalDocument.DocumentData.Length;
            IngestionTracking outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.emisLandingOrchestrationService
                .ProcessFileAsync(
                    ftpFileName: externalIngestionTracking.FileName,
                    subscriberCredential: inputSubscriberCredential,
                    supplierId: inputSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(
                    externalIngestionTracking.EncryptedFileName, It.IsAny<string>()),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = externalIngestionTracking.EncryptedFileName
            };

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    outputIngestionTracking))),
                        Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessNewNamedDocumentAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid supplierId = landingConfiguration.LandingSupplierId;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid randomIdentifier = Guid.NewGuid();
            Document randomDocument = CreateRandomDocument();
            Document externalDocument = randomDocument;
            DataSet randomDataSet = CreateRandomDataSet(supplierId);
            string randomHash = GetRandomString(64);

            IQueryable<DataSetSpecification> randomDataSetSpecificationList =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet);

            DataSetSpecification randomDataSetSpecification = randomDataSetSpecificationList.First();

            var filename = randomDocument.FileName.StartsWith('/')
                ? randomDocument.FileName
                : "/" + randomDocument.FileName;

            this.dataSetSpecificationProcessingServiceMock.Setup(service =>
                service.GetActiveDataSetSpecification(supplierId))
                    .Returns(ValueTask.FromResult(randomDataSetSpecificationList.FirstOrDefault()));

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(externalDocument.DocumentData))
                    .Returns(randomHash);

            string[] splitFileName = filename.Split('/');
            string newRandomFileName = "";

            if (splitFileName.Length < 6)
            {
                throw new InvalidDocumentProcessingFileNameException(filename);
            }
            else
            {
                newRandomFileName = $"{inputSubscriberCredential.Id}/{splitFileName[5]}/{splitFileName[6]}";
            }

            var encryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newRandomFileName}";

            var decryptedFileName =
                $"/{landingConfiguration.DecryptedFolder}" +
                $"/{randomDataSet.DataSetName}" +
                $"/{randomDataSetSpecification.Id}" +
                $"/{filename.Split('_')[2]}_{filename.Split('_')[3]}" +
                $"/{newRandomFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}";

            IngestionTracking newRandomIngestionTracking = new IngestionTracking
            {
                Id = randomIdentifier,
                FileName = externalDocument.FileName,
                SupplierId = this.landingConfiguration.LandingSupplierId,
                EncryptedFileName = encryptedFileName,
                DecryptedFileName = decryptedFileName,
                Decrypted = false,
                LastSeen = randomDateTime,
                FileDeleted = false,
                RecordCount = 0,
                EncryptedFileSize = externalDocument.DocumentData.Length,
                EncryptedFileSha256Hash = randomHash,
                DecryptedFileSize = 0,
                DecryptedFileSha256Hash = string.Empty,
                CreatedBy = "System",
                CreatedDate = randomDateTime,
                UpdatedBy = "System",
                UpdatedDate = randomDateTime
            };

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = newRandomIngestionTracking.FileName }
            };

            Download storageDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = externalDocument
            };

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            this.downloadProcessingServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
                      .ReturnsAsync(storageDownload);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomIdentifier);

            IngestionTracking outputIngestionTracking = newRandomIngestionTracking.DeepClone();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(newRandomIngestionTracking))))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.emisLandingOrchestrationService.ProcessFileAsync(
                ftpFileName: newRandomIngestionTracking.FileName,
                subscriberCredential: inputSubscriberCredential,
                supplierId: supplierId);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = newRandomIngestionTracking.EncryptedFileName
            };

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.hashBrokerMock.Verify(broker =>
            broker.GenerateSha256Hash(externalDocument.DocumentData),
            Times.Once);

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(supplierId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    newRandomIngestionTracking))),
                        Times.Once);

            this.documentProcessingServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(newBlobDocument)), It.IsAny<string>()),
                    Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}