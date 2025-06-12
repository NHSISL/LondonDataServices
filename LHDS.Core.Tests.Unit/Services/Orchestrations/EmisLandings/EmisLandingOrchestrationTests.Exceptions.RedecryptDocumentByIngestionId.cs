// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationRedecryptIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid ingestionTrackingId = Guid.NewGuid();
            var someFileName = GetRandomString();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",

                    dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask redecryptTask = this.emisLandingOrchestrationService
                .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(
                    redecryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRedecryptIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependencyException)
        {
            // given
            Guid ingestionTrackingId = Guid.NewGuid();
            var someFileName = GetRandomString();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, " +
                        "fix the errors and try again.",

                    dependencyException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask redecryptTask = this.emisLandingOrchestrationService
                .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyException>(
                    redecryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRedecryptIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            Guid ingestionTrackingId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedEmisLandingOrchestrationServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                    serviceException);

            var expectedEmisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    failedEmisLandingOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask redecryptTask = this.emisLandingOrchestrationService
                .RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(
                    redecryptTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedEmisLandingOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationServiceException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
