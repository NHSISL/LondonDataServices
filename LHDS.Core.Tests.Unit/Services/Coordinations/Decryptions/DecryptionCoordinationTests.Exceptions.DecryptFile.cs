// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnDecryptIfErrorsAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            Guid SubscriberCredentialId = Guid.NewGuid();
            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            var expectedDecryptionCoordinationDependencyValidationException =
                new DecryptionCoordinationDependencyValidationException(
                    message: "Decryption coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask<string> processDataTask =
                this.decryptionCoordinationService.DecryptAsync(filePath);

            DecryptionCoordinationDependencyValidationException
                actualDecryptionCoordinationDependencyValidationException =
                    await Assert.ThrowsAsync<DecryptionCoordinationDependencyValidationException>(
                        processDataTask.AsTask);

            // Then
            actualDecryptionCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnDecryptIfErrorsAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            Guid SubscriberCredentialId = Guid.NewGuid();
            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyValidationException);

            var expectedDecryptionCoordinationDependencyException =
                new DecryptionCoordinationDependencyException(
                    message: "Decryption coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask<string> processDataTask = this.decryptionCoordinationService.DecryptAsync(filePath);

            DecryptionCoordinationDependencyException actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<DecryptionCoordinationDependencyException>(async () =>
                    await processDataTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationDependencyException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDecryptIfErrorsAndLogItAsync()
        {
            // Given
            var serviceException = new Exception();
            Guid SubscriberCredentialId = Guid.NewGuid();
            string encryptedFileName = CreateRandomFilePath(SubscriberCredentialId);
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(serviceException);

            var failedDecryptionCoordinationServiceException =
                new FailedDecryptionCoordinationServiceException(
                    message: "Failed decryption coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecryptionCoordinationServiceException =
                new DecryptionCoordinationServiceException(
                    message: "Decryption coordination service error occurred, please contact support.",
                    innerException: failedDecryptionCoordinationServiceException);

            // When
            ValueTask<string> processDataTask = this.decryptionCoordinationService.DecryptAsync(encryptedFileName);

            DecryptionCoordinationServiceException actualDecryptionCoordinationServiceException =
                await Assert.ThrowsAsync<DecryptionCoordinationServiceException>(async () =>
                    await processDataTask);

            // Then
            actualDecryptionCoordinationServiceException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

