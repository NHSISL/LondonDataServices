// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Decryptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfBlobContainerIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string someFileName = GetRandomString();

            var nullBlobContainersDecryptionOrchestrationException =
                new NullBlobContainersDecryptionOrchestrationException(
                    message: "Null blob container decryption orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDecryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersDecryptionOrchestrationException);

            BlobContainers invalidBlobContainers = null;

            var invalidDecryptionOrchestrationService = new DecryptionOrchestrationService(
                documentService: documentServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                cryptographyService: cryptographyServiceMock.Object,
                ingestionTrackingService: ingestionTrackingServiceMock.Object,
                auditService: auditServiceMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                hashBroker: hashBrokerMock.Object
                );

            // when
            ValueTask<string> decryptTask =
                invalidDecryptionOrchestrationService.DecryptAsync(someFileName, inputSubscriberCredential);

            DecryptionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecryptionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnDecryptIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            var invalidArgumentDecryptionOrchestrationException =
                new InvalidArgumentDecryptionOrchestrationException(
                    message: "Invalid decryption orchestration argument(s), please correct the errors and try again.");

            invalidArgumentDecryptionOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedDecryptionOrchestrationFileNameValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentDecryptionOrchestrationException);

            // when
            ValueTask<string> decryptTask =
                this.decryptionOrchestrationService.DecryptAsync(invalidText, inputSubscriberCredential);

            DecryptionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecryptionOrchestrationValidationException>(decryptTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecryptionOrchestrationFileNameValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationFileNameValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfSubscriptionCredentialIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential inputSubscriberCredential = null;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var nullSubscriberCredentialDecryptionOrchestrationException =
                new NullSubscriberCredentialDecryptionOrchestrationException(
                    message: "Null subscriber credential decryption orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDecryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialDecryptionOrchestrationException);

            // when
            ValueTask<string> processTask =
                this.decryptionOrchestrationService
                    .DecryptAsync(encryptedFileName: inputFileName, subscriberCredential: inputSubscriberCredential);

            DecryptionOrchestrationValidationException actualDecryptionOrchestrationValidationException =
                await Assert.ThrowsAsync<DecryptionOrchestrationValidationException>(processTask.AsTask);

            // then
            actualDecryptionOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDecryptionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.cryptographyServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentOnProcessFileIfDocumentIsNullAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.FileName = randomFileName;
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            Stream outputStream = new MemoryStream();
            Stream storageStream = new MemoryStream();

            var notFoundDecryptionOrchestrationException =
                new NotFoundDecryptionOrchestrationException(
                message: $"Couldn't find document with file name: {inputFileName}.");

            var expectedDecryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: notFoundDecryptionOrchestrationException);

            this.ingestionTrackingServiceMock.Setup(service =>
               service.RetrieveIngestionTrackingByEncryptedFileNameAsync(randomFileName))
                   .ReturnsAsync(storageIngestionTracking);

            this.documentServiceMock
                .Setup(service => service.RetrieveDocumentByFileNameAsync(
                     storageStream,
                     storageIngestionTracking.EncryptedFileName,
                     It.IsAny<string>()))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    storageStream.Position = 0;
                    storageStream.CopyTo(outputStream);
                })
                .Returns(ValueTask.CompletedTask);

            // when
            ValueTask<string> processTask = this.decryptionOrchestrationService
                .DecryptAsync(encryptedFileName: inputFileName, subscriberCredential: inputSubscriberCredential);

            DecryptionOrchestrationValidationException actualDecryptionOrchestrationValidationExceptionn =
                await Assert.ThrowsAsync<DecryptionOrchestrationValidationException>(processTask.AsTask);

            // then
            actualDecryptionOrchestrationValidationExceptionn.Should()
                .BeEquivalentTo(expectedDecryptionOrchestrationValidationException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByEncryptedFileNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.RetrieveDocumentByFileNameAsync(
                    It.IsAny<Stream>(),
                    storageIngestionTracking.EncryptedFileName,
                    It.IsAny<string>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDecryptionOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.cryptographyServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}
