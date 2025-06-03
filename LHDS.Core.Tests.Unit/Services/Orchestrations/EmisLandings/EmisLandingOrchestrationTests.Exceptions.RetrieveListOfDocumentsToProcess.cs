// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DownloadDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveListIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            Download someDownload = new Download { SubscriberCredential = someSubscriberCredential };

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",

                    dependancyValidationException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()))
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask<List<string>> retrieveListOfDocumentsToProcessAsyncTask = this.emisLandingOrchestrationService
                .RetrieveListOfDocumentsToProcessAsync(subscriberCredential: someSubscriberCredential);

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(
                    retrieveListOfDocumentsToProcessAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
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

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveListIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            Guid inputSupplierId = Guid.NewGuid();
            Download someDownload = new Download { SubscriberCredential = someSubscriberCredential };

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<string>> retrieveListOfDocumentsToProcessAsyncTask = this.emisLandingOrchestrationService
                .ProcessAsync(subscriberCredential: someSubscriberCredential, supplierId: inputSupplierId);

            EmisLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyException>(
                    retrieveListOfDocumentsToProcessAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveListIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            Download someDownload = new Download { SubscriberCredential = someSubscriberCredential };
            var serviceException = new Exception();
            Guid inputSupplierId = Guid.NewGuid();

            var failedEmisLandingOrchestrationServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                    serviceException);

            var expectedEmisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    failedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> retrieveListOfDocumentsToProcessAsyncTask = this.emisLandingOrchestrationService
                .ProcessAsync(subscriberCredential: someSubscriberCredential, supplierId: inputSupplierId);

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(
                    retrieveListOfDocumentsToProcessAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.IsAny<Download>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationServiceException))),
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
