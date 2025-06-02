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
            using var storageStream = new MemoryStream(randomEncryptedBytes);
            var encryptedStreamStream = new MemoryStream();
            var decryptedStream = new MemoryStream();
            var documentAddedToBlobstoreStream = new MemoryStream();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                    .ReturnsAsync(storageIngestionTracking);

            string batchCompleteFileName =
                $"{storageIngestionTracking.BatchReadyFolderPath}/{landingConfiguration.BatchReadyFile}".Replace("\\", "/");

            this.documentServiceMock.Setup(service => service.RemoveDocumentByFileNameAsync(
                batchCompleteFileName,
                this.blobContainers.Ingress))
                    .Returns(ValueTask.CompletedTask);

            this.documentServiceMock
                .Setup(service =>
                    service.RetrieveDocumentByFileNameAsync(
                        It.IsAny<Stream>(),
                        storageIngestionTracking.EncryptedFileName,
                        It.IsAny<string>()))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    storageStream.Position = 0;
                    storageStream.CopyTo(encryptedStreamStream);
                    encryptedStreamStream.Position = 0;
                    encryptedStreamStream.CopyTo(stream);
                    stream.Flush();
                })
                .Returns(ValueTask.CompletedTask);

            this.cryptographyServiceMock
                .Setup(service => service.DecryptAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<Stream>(),
                    randomSubscriberCredential))
                .Callback<Stream, Stream, SubscriberCredential>((input, output, subscriberCredential) =>
                {
                    using var simulatedDecryptedContent = new MemoryStream(randomDecryptedBytes);
                    simulatedDecryptedContent.Position = 0;
                    simulatedDecryptedContent.CopyTo(decryptedStream);
                    decryptedStream.Position = 0;
                    decryptedStream.CopyTo(output);
                    output.Flush();
                })
                .Returns(ValueTask.CompletedTask);

            this.hashBrokerMock.Setup(broker =>
                broker.GenerateSha256HashAsync(It.IsAny<Stream>()))
                    .ReturnsAsync(randomHash);

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    storageIngestionTracking.DecryptedFileName,
                    It.IsAny<string>()))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    stream.Position = 0;
                    stream.CopyTo(documentAddedToBlobstoreStream);
                })
                .Returns(ValueTask.CompletedTask);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var updatedIngestionTracking = storageIngestionTracking.DeepClone();
            updatedIngestionTracking.Decrypted = true;
            updatedIngestionTracking.RecordCount = 0;
            updatedIngestionTracking.DecryptedFileSize = randomDecryptedBytes.Length;
            updatedIngestionTracking.DecryptedFileSha256Hash = randomHash;
            updatedIngestionTracking.IsProcessing = false;
            updatedIngestionTracking.UpdatedDate = randomDateTimeOffset;
            var outputIngestionTracking = updatedIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(updatedIngestionTracking))
                    .ReturnsAsync(outputIngestionTracking);

            // when
            var result = await this.decryptionOrchestrationService.DecryptAsync(randomFileName, inputSubscriberCredential);

            // then

            result.DecryptedFileName.Should().Be(storageIngestionTracking.DecryptedFileName);
            result.IngestionTrackingId.Should().Be(storageIngestionTracking.Id);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName),
                Times.Once);

            this.documentServiceMock.Verify(service => service.RemoveDocumentByFileNameAsync(
                batchCompleteFileName,
                this.blobContainers.Ingress),
                    Times.Once);

            this.documentServiceMock
                .Verify(service =>
                    service.RetrieveDocumentByFileNameAsync(
                        It.IsAny<Stream>(),
                        storageIngestionTracking.EncryptedFileName,
                        It.IsAny<string>()),
                Times.Once);

            encryptedStreamStream.Position = 0;
            ReadAllBytesFromStream(encryptedStreamStream).Should().BeEquivalentTo(randomEncryptedBytes);

            this.cryptographyServiceMock
                .Verify(service => service.DecryptAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<Stream>(),
                    randomSubscriberCredential),
                Times.Once);

            decryptedStream.Position = 0;
            ReadAllBytesFromStream(decryptedStream).Should().BeEquivalentTo(randomDecryptedBytes);

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
                Times.Exactly(2));

            this.auditServiceMock.Verify(service =>
                service.AddIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                Times.Exactly(2));

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
