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
        public async Task ShouldThrowDependencyValidationExceptionOnProcessDecryptedItemsForBatchCompleteIfErrorsAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            var expectedDecryptionCoordinationDependencyValidationException =
                new DecryptionCoordinationDependencyValidationException(
                    message: "Decryption coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.ingressOrchestrationServiceMock.Setup(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync())
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask processDataTask =
                this.decryptionCoordinationService.ProcessDecryptedItemsForBatchCompleteAsync();

            DecryptionCoordinationDependencyValidationException
                actualDecryptionCoordinationDependencyValidationException =
                    await Assert.ThrowsAsync<DecryptionCoordinationDependencyValidationException>(
                        processDataTask.AsTask);

            // Then
            actualDecryptionCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationDependencyValidationException);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessDecryptedItemsForBatchCompleteIfErrorsAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            this.ingressOrchestrationServiceMock.Setup(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync())
                    .ThrowsAsync(dependancyValidationException);

            var expectedDecryptionCoordinationDependencyException =
                new DecryptionCoordinationDependencyException(
                    message: "Decryption coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask processDataTask = this.decryptionCoordinationService
                .ProcessDecryptedItemsForBatchCompleteAsync();

            DecryptionCoordinationDependencyException actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<DecryptionCoordinationDependencyException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationDependencyException);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessDecryptedItemsForBatchCompleteIfErrorsAndLogItAsync()
        {
            // Given
            var serviceException = new Exception();
            Guid SubscriberCredentialId = Guid.NewGuid();
            string encryptedFileName = CreateRandomFilePath(SubscriberCredentialId);
            List<Exception> exceptions = new List<Exception>();

            this.ingressOrchestrationServiceMock.Setup(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync())
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
            ValueTask processDataTask = this.decryptionCoordinationService
                .ProcessDecryptedItemsForBatchCompleteAsync();

            DecryptionCoordinationServiceException actualDecryptionCoordinationServiceException =
                await Assert.ThrowsAsync<DecryptionCoordinationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualDecryptionCoordinationServiceException.Should()
                .BeEquivalentTo(expectedDecryptionCoordinationServiceException);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedDecryptionCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

