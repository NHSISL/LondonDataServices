// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRedecryptIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            Guid ingestionTrackingId = Guid.NewGuid();

            this.emisLandingOrchestrationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId))
                    .ThrowsAsync(dependencyValidationException);

            var expectedEmisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            // When
            ValueTask redecryptTask =
                this.emisLandingCoordinationService.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingCoordinationDependencyValidationException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyValidationException>(
                    redecryptTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyValidationException);

            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyValidationException))),
                         Times.Once);

            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRedecryptIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            Guid ingestionTrackingId = Guid.NewGuid();

            this.emisLandingOrchestrationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId))
                    .ThrowsAsync(dependencyException);

            var expectedEmisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            // When
            ValueTask redecryptTask =
                this.emisLandingCoordinationService.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingCoordinationDependencyException actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyException>(
                    redecryptTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyException);

            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyException))),
                         Times.Once);

            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRedecryptIfErrorsInLoopAndLogItAsync()
        {
            // Given
            Guid ingestionTrackingId = Guid.NewGuid();
            var serviceException = new Exception();

            this.emisLandingOrchestrationServiceMock.Setup(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId))
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
            ValueTask redecryptTask =
                this.emisLandingCoordinationService.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    redecryptTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.emisLandingOrchestrationServiceMock.Verify(service =>
                service.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

