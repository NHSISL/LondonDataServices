// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true))
                    .ThrowsAsync(dependencyValidationException);

            var expectedEmisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            // When
            ValueTask<Document> retrieveDownlaodTask =
                this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(fileName);

            EmisLandingCoordinationDependencyValidationException
                actualEmisLandingCoordinationDependencyValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyValidationException>(async () =>
                    await retrieveDownlaodTask);

            // Then
            actualEmisLandingCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true))
                    .ThrowsAsync(dependencyException.InnerException as Xeption);

            var expectedEmisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, please try again.",
                    innerException: dependencyException);

            // When
            ValueTask<Document> retrieveDownlaodTask =
                this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(fileName);

            EmisLandingCoordinationServiceException
                actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(async () =>
                    await retrieveDownlaodTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfErrorsInLoopAndLogItAsync()
        {
            // Given
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);
            var serviceException = new Exception();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true))
                    .ThrowsAsync(serviceException);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service occurred, please contact support.",
                    innerException: serviceException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<Document> retrieveDownloadTask =
                this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(fileName);

            EmisLandingCoordinationServiceException
                actualEmisLandingCoordinationServiceException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(async () =>
                    await retrieveDownloadTask);

            // Then
            actualEmisLandingCoordinationServiceException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(subscriberCredentialId, true),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}

