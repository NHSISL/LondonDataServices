// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;
using Document = LHDS.Core.Models.Foundations.Documents.Document;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{

    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedDocumentAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomMessage();
            byte[] randomEncryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            byte[] randomDecryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.FileName = randomFileName;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            string randomHash = GetRandomString(64);

            Document randomDocument =
                new Document { FileName = randomFileName, DocumentData = randomEncryptedBytes };

            Document encryptedDocument = randomDocument;

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = randomIngestionTracking.FileName }
            };

            Download storageDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = encryptedDocument
            };

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByFileNameAsync(randomFileName))
                   .ReturnsAsync(storageIngestionTracking);

            this.downloadProcessingServiceMock.Setup(service =>
                   service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
                       .ReturnsAsync(storageDownload);

            this.cryptographyServiceMock.Setup(service =>
                service.DecryptAsync(storageDownload.Document.DocumentData))
                    .ReturnsAsync(randomDecryptedBytes);

            this.hashBrokerMock.Setup(broker =>
               broker.GenerateSha256Hash(randomDecryptedBytes))
                   .Returns(randomHash);

            string[] lines = Encoding.UTF8.GetString(randomDecryptedBytes)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            updatedIngestionTracking.RecordCount = lines.Length - 2;
            updatedIngestionTracking.DecryptedFileSize = storageDownload.Document.DocumentData.Length;
            updatedIngestionTracking.DecryptedFileSha256Hash = randomHash;
            updatedIngestionTracking.UpdatedDate = randomDateTimeOffset;

            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName, inputSubscriberCredential);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
              service.RetrieveIngestionTrackingByFileNameAsync(randomDocument.FileName),
                  Times.Once);

            this.downloadProcessingServiceMock.Verify(service =>
                 service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
                     Times.Once);

            this.cryptographyServiceMock.Verify(service =>
                service.DecryptAsync(encryptedDocument.DocumentData),
                    Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(randomDecryptedBytes),
                    Times.Once);

            Document newBlobDocument = new Document
            {
                DocumentData = randomDecryptedBytes,
                FileName = storageIngestionTracking.DecryptedFileName
            };

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(newBlobDocument)),
                    It.IsAny<string>()),
                        Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
