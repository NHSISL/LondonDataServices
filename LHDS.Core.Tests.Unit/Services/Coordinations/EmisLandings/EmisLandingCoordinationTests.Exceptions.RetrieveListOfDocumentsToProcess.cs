// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveFileListIfErrorsAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            Guid subscriberCredentialId = Guid.NewGuid();

            var expectedEmisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask<List<string>> retrieveListOfDocumentsToProcessTask =
                this.emisLandingCoordinationService
                    .RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId: subscriberCredentialId);

            EmisLandingCoordinationDependencyValidationException
                actuaEmisLandingCoordinationDependencyValidationException =
                    await Assert.ThrowsAsync<EmisLandingCoordinationDependencyValidationException>(
                        retrieveListOfDocumentsToProcessTask.AsTask);

            // Then
            actuaEmisLandingCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveFileListIfErrorsAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            Guid subscriberCredentialId = Guid.NewGuid();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                    .ThrowsAsync(dependancyValidationException);

            var expectedEmisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask<List<string>> retrieveListOfDocumentsToProcessTask =
                this.emisLandingCoordinationService
                    .RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId: subscriberCredentialId);

            EmisLandingCoordinationDependencyException actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyException>(async () =>
                    await retrieveListOfDocumentsToProcessTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveFileListIfErrorsAndLogItAsync()
        {
            // Given
            var serviceException = new Exception();
            Guid subscriberCredentialId = Guid.NewGuid();
            List<Exception> exceptions = new List<Exception>();

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
            ValueTask<List<string>> retrieveListOfDocumentsToProcessTask =
                this.emisLandingCoordinationService
                    .RetrieveListOfDocumentsToProcessAsync(subscriberAgreementId: subscriberCredentialId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationServiceException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(async () =>
                    await retrieveListOfDocumentsToProcessTask);

            // Then
            actualEmisLandingCoordinationServiceException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

