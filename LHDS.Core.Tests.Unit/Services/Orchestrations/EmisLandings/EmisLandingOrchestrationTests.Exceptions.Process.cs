// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            Guid someSupplierId = Guid.NewGuid();

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            Exception someProcessException =
                new Exception(message: "Failed to process subscriber files.");

            Exception someDeletionException =
                new Exception(message: "Failed to mark items as deleted.");

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessSubscriberFiles(It.IsAny<SubscriberCredential>(), It.IsAny<Guid>()))
                    .ThrowsAsync(someProcessException);

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.MarkItemsAsDeleteThatHasNotBeenSeen(someSubscriberCredential.Id))
                    .ThrowsAsync(someDeletionException);

            AggregateException aggregateException = new AggregateException(
                $"Unable to process documents",
                someProcessException,
                someDeletionException);

            var failedDownloadServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                    aggregateException);

            EmisLandingOrchestrationServiceException expectedException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    innerException: failedDownloadServiceException);

            // when
            ValueTask<List<string>> processTask = emisLandingOrchestrationServiceMock.Object
                .ProcessAsync(subscriberCredential: someSubscriberCredential, supplierId: someSupplierId);

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessSubscriberFiles(It.IsAny<SubscriberCredential>(), It.IsAny<Guid>()),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.MarkItemsAsDeleteThatHasNotBeenSeen(someSubscriberCredential.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedException))),
                        Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
