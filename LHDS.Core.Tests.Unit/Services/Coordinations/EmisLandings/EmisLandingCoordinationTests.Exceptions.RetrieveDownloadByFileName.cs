// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
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
            Stream outputStream = new MemoryStream();
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);

            var expectedEmisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependencyValidationException);

            // When
            ValueTask retrieveDownloadTask = this.emisLandingCoordinationService
                .RetrieveDownloadByFileNameAsync(output: outputStream, fileName);

            EmisLandingCoordinationDependencyValidationException
                actualEmisLandingCoordinationDependencyValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyValidationException>(
                    retrieveDownloadTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
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
            Stream outputStream = new MemoryStream();
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);

            var expectedEmisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependencyException);

            // When
            ValueTask retrieveDownloadTask = this.emisLandingCoordinationService
                .RetrieveDownloadByFileNameAsync(output: outputStream, fileName);

            EmisLandingCoordinationDependencyException
                actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyException>(
                    retrieveDownloadTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
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
            Stream outputStream = new MemoryStream();
            Guid subscriberCredentialId = Guid.NewGuid();
            string fileName = CreateRandomSubscriberCredentialIdFileName(subscriberCredentialId);
            var serviceException = new Exception();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(serviceException);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask retrieveDownloadTask = this.emisLandingCoordinationService
                .RetrieveDownloadByFileNameAsync(output: outputStream, fileName);

            EmisLandingCoordinationServiceException
                actualEmisLandingCoordinationServiceException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    retrieveDownloadTask.AsTask);

            // Then
            actualEmisLandingCoordinationServiceException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}

