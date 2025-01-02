// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            byte[] randomEncryptedBytes = Encoding.UTF8.GetBytes(GetRandomMessage());
            byte[] randomDecryptedBytes = Encoding.UTF8.GetBytes(GetRandomMessage());
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.FileName = randomFileName;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            string randomHash = GetRandomString(64);
            Stream storageStream = new MemoryStream(randomEncryptedBytes);
            Stream retrievedDocument = new MemoryStream();
            Stream decryptedContent = new MemoryStream(randomDecryptedBytes);
            Stream decryptedDocument = new MemoryStream();
            Stream documentAddedToBlobstore = new MemoryStream();

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                   .ReturnsAsync(storageIngestionTracking);

            this.documentServiceMock
                .Setup(service =>
                    service.RetrieveDocumentByFileNameAsync(
                        It.Is(SameStreamAs(retrievedDocument)),
                        storageIngestionTracking.EncryptedFileName,
                        It.IsAny<string>()))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    storageStream.Position = 0;
                    storageStream.CopyTo(retrievedDocument);
                    storageStream.Position = 0;
                    storageStream.CopyTo(stream);
                })
                .Returns(ValueTask.CompletedTask);

            this.cryptographyServiceMock
                .Setup(service => service.DecryptAsync(
                    It.Is(SameStreamAs(retrievedDocument)),
                    It.Is(SameStreamAs(decryptedDocument)),
                    randomSubscriberCredential))
                .Callback<Stream, Stream, SubscriberCredential>((input, output, subscriberCredential) =>
                {
                    decryptedContent.Position = 0;
                    decryptedContent.CopyTo(decryptedDocument);
                    decryptedContent.Position = 0;
                    decryptedContent.CopyTo(output);
                })
                .Returns(ValueTask.CompletedTask);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256HashAsync(It.Is(SameStreamAs(decryptedDocument))))
                    .ReturnsAsync(randomHash);

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.Is(SameStreamAs(decryptedDocument)),
                    storageIngestionTracking.DecryptedFileName,
                    It.IsAny<string>()))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    decryptedContent.Position = 0;
                    decryptedContent.CopyTo(documentAddedToBlobstore);
                })
                .Returns(ValueTask.CompletedTask);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            updatedIngestionTracking.RecordCount = 0;
            updatedIngestionTracking.DecryptedFileSize = decryptedContent.Length;
            updatedIngestionTracking.DecryptedFileSha256Hash = randomHash;
            updatedIngestionTracking.IsProcessing = false;
            updatedIngestionTracking.UpdatedDate = randomDateTimeOffset;
            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            await this.decryptionOrchestrationService.DecryptAsync(randomFileName, inputSubscriberCredential);

            // then
            this.ingestionTrackingServiceMock.Verify(service =>
               service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName),
                  Times.Once);

            this.documentServiceMock
                .Verify(service =>
                    service.RetrieveDocumentByFileNameAsync(
                        It.IsAny<Stream>(),
                        storageIngestionTracking.EncryptedFileName,
                        It.IsAny<string>()),
                            Times.Once);

            ReadAllBytesFromStream(retrievedDocument).Should().BeEquivalentTo(randomEncryptedBytes);

            this.cryptographyServiceMock
                .Verify(service => service.DecryptAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<Stream>(),
                    randomSubscriberCredential),
                        Times.Once);

            ReadAllBytesFromStream(decryptedDocument).Should().BeEquivalentTo(randomDecryptedBytes);

            this.documentServiceMock
                .Verify(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    storageIngestionTracking.DecryptedFileName,
                    It.IsAny<string>()),
                Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
               service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(updatedIngestionTracking))),
                   Times.Once);

            this.hashBrokerMock.Verify(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
