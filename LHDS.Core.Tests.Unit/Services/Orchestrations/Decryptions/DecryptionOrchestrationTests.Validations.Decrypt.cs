// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
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
                invalidDecryptionOrchestrationService.DecryptAsync(someFileName);

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
                this.decryptionOrchestrationService.DecryptAsync(invalidText);

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
    }
}
