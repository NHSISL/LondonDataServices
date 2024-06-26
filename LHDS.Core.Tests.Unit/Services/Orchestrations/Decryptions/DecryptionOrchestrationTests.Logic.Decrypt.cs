// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

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

            Document randomDocument = new Document
            {
                FileName = randomFileName,
                //DocumentData = randomEncryptedBytes 
            };

            Document encryptedDocument = randomDocument;

            Document decryptedDocument = new Document
            {
                //DocumentData = randomDecryptedBytes,
                FileName = storageIngestionTracking.DecryptedFileName
            };

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256Hash(decryptedDocument.DocumentData))
                    .Returns(randomHash);

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                   .ReturnsAsync(storageIngestionTracking);

            //this.documentServiceMock.Setup(service =>
            //    service.RetrieveDocumentByFileNameAsync(
            //        storageIngestionTracking.EncryptedFileName,
            //        It.IsAny<string>()))
            //            .ReturnsAsync(encryptedDocument);

            //this.cryptographyServiceMock.Setup(service =>
            //    service.DecryptAsync(encryptedDocument.DocumentData, randomSubscriberCredential))
            //        .ReturnsAsync(randomDecryptedBytes);

            string[] lines = Encoding.UTF8.GetString(randomDecryptedBytes)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            updatedIngestionTracking.RecordCount = lines.Length - 2;
            updatedIngestionTracking.DecryptedFileSize = decryptedDocument.DocumentData.Length;
            updatedIngestionTracking.DecryptedFileSha256Hash = randomHash;
            updatedIngestionTracking.UpdatedDate = randomDateTimeOffset;
            updatedIngestionTracking.IsProcessing = false;

            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName, inputSubscriberCredential);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
              service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomDocument.FileName),
                  Times.Once);

            //this.documentServiceMock.Verify(service =>
            //    service.RetrieveDocumentByFileNameAsync(
            //        storageIngestionTracking.EncryptedFileName,
            //        It.IsAny<string>()),
            //            Times.Once);

            //this.cryptographyServiceMock.Verify(service =>
            //    service.DecryptAsync(encryptedDocument.DocumentData, randomSubscriberCredential),
            //        Times.Once);

            //this.documentServiceMock.Verify(service =>
            //    service.AddDocumentAsync(
            //        It.Is(SameDocumentAs(decryptedDocument)),
            //        It.IsAny<string>()),
            //            Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
               service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                   Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256Hash(decryptedDocument.DocumentData),
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
