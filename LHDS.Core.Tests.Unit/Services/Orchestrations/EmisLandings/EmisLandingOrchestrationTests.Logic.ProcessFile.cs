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

            List<IngestionTracking> externalIngestionTrackingsFound =
                new List<IngestionTracking> { externalIngestionTracking };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            this.downloadProcessingServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(externalIngestionTracking.FileName))
                      .ReturnsAsync(externalDocument);

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
                    fileName: externalIngestionTracking.FileName,
                    subscriberCredential: inputSubscriberCredential);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(externalIngestionTracking.FileName),
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

            IngestionTracking newRandomIngestionTracking = new IngestionTracking
            {
                Id = randomIdentifier,
                FileName = externalDocument.FileName,
                SupplierId = this.landingConfiguration.LandingSupplierId,
                EncryptedFileName = $"/{landingConfiguration.EncryptedFolder}/{newRandomFileName}",

                DecryptedFileName =
                    $"/{landingConfiguration.DecryptedFolder}"
                    + $"/{randomDataSet.DataSetName}"
                    + $"/{randomDataSetSpecification.Id}"
                    + $"/{filename.Split('_')[3]}"
                    + $"/{newRandomFileName.Replace(".gpg", "", StringComparison.InvariantCultureIgnoreCase)}",

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

            List<IngestionTracking> externalIngestionTrackingsFound = new List<IngestionTracking>();

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Returns(externalIngestionTrackingsFound.AsQueryable());

            this.downloadProcessingServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(newRandomIngestionTracking.FileName))
                      .ReturnsAsync(externalDocument);

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
                fileName: newRandomIngestionTracking.FileName,
                subscriberCredential: inputSubscriberCredential);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(newRandomIngestionTracking.FileName),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDocument.DocumentData,
                FileName = newRandomIngestionTracking.EncryptedFileName
            };

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(
                    outputIngestionTracking))),
                        Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(externalDocument.DocumentData),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            this.dataSetSpecificationProcessingServiceMock.Verify(service =>
                service.GetActiveDataSetSpecification(supplierId),
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