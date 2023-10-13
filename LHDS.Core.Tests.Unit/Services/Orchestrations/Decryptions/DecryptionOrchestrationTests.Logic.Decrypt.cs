// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            var randomContainer = GetRandomMessage();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomMessage();
            byte[] randomEncryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            byte[] randomDecryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.FileName = randomFileName;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;

            Document randomDocument =
                new Document { FileName = randomFileName, DocumentData = randomEncryptedBytes };

            Document encryptedDocument = randomDocument;

            Document decryptedDocument = new Document
            {
                DocumentData = randomDecryptedBytes,
                FileName = storageIngestionTracking.DecryptedFileName
            };

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByFileNameAsync(randomFileName))
                   .ReturnsAsync(storageIngestionTracking);

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(
                    storageIngestionTracking.EncryptedFileName,
                    randomContainer))
                        .ReturnsAsync(encryptedDocument);

            this.decryptionServiceMock.Setup(service =>
                service.DecryptAsync(encryptedDocument.DocumentData))
                    .ReturnsAsync(randomDecryptedBytes);

            string[] lines = Encoding.UTF8.GetString(randomDecryptedBytes)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            updatedIngestionTracking.RecordCount = lines.Length - 2;
            updatedIngestionTracking.DecryptedFileSize = decryptedDocument.DocumentData.Length;
            updatedIngestionTracking.UpdatedDate = randomDateTimeOffset;

            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
              service.RetrieveIngestionTrackingByFileNameAsync(randomDocument.FileName),
                  Times.Once);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(
                    storageIngestionTracking.EncryptedFileName,
                    randomContainer),
                        Times.Once);

            this.decryptionServiceMock.Verify(service =>
                service.DecryptAsync(encryptedDocument.DocumentData),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.Is(SameDocumentAs(decryptedDocument)),
                    randomContainer),
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

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
