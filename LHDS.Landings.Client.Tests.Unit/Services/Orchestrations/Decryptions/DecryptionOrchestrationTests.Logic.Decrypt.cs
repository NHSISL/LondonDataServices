// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using Moq;
using Document = LHDS.Landings.Client.Models.Foundations.Documents.Document;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Decryptions
{

    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedDocumentAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            string randomFileName = GetRandomMessage();
            byte[] randomEncryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            byte[] randomDecryptedBytes = Encoding.ASCII.GetBytes(GetRandomMessage());
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();

            Document randomDocument =
                new Document { FileName = randomFileName, DocumentData = randomEncryptedBytes };

            Document encryptedDocument = randomDocument;

            Document decryptedDocument = new Document
            {
                DocumentData = randomDecryptedBytes,
                FileName = storageIngestionTracking.DecryptedFileName
            };

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByIdAsync(randomDocument.FileName))
                   .ReturnsAsync(storageIngestionTracking);

            this.documentServiceMock.Setup(service =>
                service.RetrieveDocumentByFileNameAsync(storageIngestionTracking.EncryptedFileName))
                    .ReturnsAsync(encryptedDocument);

            this.decryptionServiceMock.Setup(service =>
                service.DecryptAsync(encryptedDocument.DocumentData))
                    .ReturnsAsync(randomDecryptedBytes);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
              service.RetrieveIngestionTrackingByIdAsync(randomDocument.FileName),
                  Times.Once);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(storageIngestionTracking.EncryptedFileName),
                    Times.Once);

            this.decryptionServiceMock.Verify(service =>
                service.DecryptAsync(encryptedDocument.DocumentData),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.Is(SameDocumentAs(decryptedDocument))),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.auditServiceMock.Verify(service =>
                service.AddAuditAsync(It.IsAny<Audit>()),
                    Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
