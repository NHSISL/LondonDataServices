// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(TppDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnProcessIfDependencyValidationOccursAndLogItAsync(
             Xeption dependancyValidationException)
        {
            // given
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();

            var expectedDependencyException =
                new TppLandingOrchestrationDependencyValidationException(
                    message: "TPP landing orchestration dependency validation error occurred, fix the errors and try again.",
                    dependancyValidationException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependancyValidationException);

            // when
            ValueTask<Guid> processTask = this.tppOrchestrationService.ProcessAsync(randomDocument);

            TppLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                       Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TppDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfDependencyExceptionOccursAndLogItAsync(
          Xeption dependancyException)
        {
            // given
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();

            var expectedDependencyException =
                new TppLandingOrchestrationDependencyException(
                    message: "TPP landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependancyException);

            // when
            ValueTask<Guid> processTask = this.tppOrchestrationService.ProcessAsync(randomDocument);

            TppLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            Models.Foundations.Documents.Document randomDocument = CreateRandomDocument();
            var serviceException = new Exception();

            var failedTppOrchestrationServiceException =
                new FailedTppLandingOrchestrationServiceException(
                    message: "Failed TPP landing orchestration service occurred, please contact support",
                    serviceException);

            var expectedTppOrchestrationServiceException =
                new TppLandingOrchestrationServiceException(
                    message: "TPP landing orchestration service error occurred, contact support.",
                    failedTppOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(serviceException);

            // when
            ValueTask<Guid> processTask = this.tppOrchestrationService.ProcessAsync(randomDocument);

            TppLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTppOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTppOrchestrationServiceException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}