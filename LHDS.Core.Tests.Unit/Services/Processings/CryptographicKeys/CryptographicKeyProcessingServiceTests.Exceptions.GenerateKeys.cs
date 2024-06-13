// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.CryptographicKeys.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.CryptographicKeys
{
    public partial class CryptographicKeyProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnGenerateKeysIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            var expectedDependencyException =
                new CryptographicKeyProcessingDependencyValidationException(
                    message: "Cryptography key processing dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberCredential> generateKeysTask =
                this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            CryptographicKeyProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<CryptographicKeyProcessingDependencyValidationException>(
                    generateKeysTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnGenerateKeysIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;

            var expectedDependencyException =
                new CryptographicKeyProcessingDependencyException(
                    message: "Cryptographic key processing dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberCredential> generateKeysTask =
                this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            CryptographicKeyProcessingDependencyException actualException =
                await Assert.ThrowsAsync<CryptographicKeyProcessingDependencyException>(
                    generateKeysTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGenerateKeysIfServiceErrorOccursAndLogItAsync()
        {
            //given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            var serviceException = new Exception();

            var failedSubscriberCredentialOrchestrationServiceException =
                new FailedCryptographicKeyProcessingServiceException(
                    message: "Failed cryptography key processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDependencyException =
                new CryptographicKeyProcessingServiceException(
                    message: "Cryptography key processing service error occurred, please contact support.",
                    innerException: failedSubscriberCredentialOrchestrationServiceException);

            this.cryptographyKeyServiceMock.Setup(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberCredential> generateKeysTask =
                this.cryptographyKeyProcessingService.GenerateKeysAsync(inputSubscriberCredential);

            CryptographicKeyProcessingServiceException actualException =
                await Assert.ThrowsAsync<CryptographicKeyProcessingServiceException>(
                    generateKeysTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.cryptographyKeyServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                       Times.Once);

            this.cryptographyKeyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
