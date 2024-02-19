// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnProcessIfErrorsAndLogItAsync(
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
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId))
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
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfErrorsAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            Guid SubscriberCredentialId = Guid.NewGuid();
            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RemoveSubscriberCredentialByIdAsync(SubscriberCredentialId))
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
                service.RetrieveAllActiveSubscriberCredentialIds(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

