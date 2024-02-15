// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnProcessFileIfDependencyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            var someFileName = GetRandomMessage();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",

                    dependancyValidationException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService
                .ProcessFileAsync(fileName: someFileName, subscriberCredential: someSubscriberCredential);

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessFileIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            string someFileName = GetRandomMessage();

            var expectedDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService
                .ProcessFileAsync(fileName: someFileName, subscriberCredential: someSubscriberCredential);

            EmisLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessFileIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();
            var someFileName = GetRandomMessage();
            var serviceException = new Exception();

            var failedEmisLandingOrchestrationServiceException =
                new FailedEmisLandingOrchestrationServiceException(
                    message: "Failed EMIS landing orchestration service occurred, please contact support",
                    serviceException);

            var expectedEmisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, contact support.",
                    failedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName))
                    .Throws(serviceException);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService
                .ProcessFileAsync(fileName: someFileName, subscriberCredential: someSubscriberCredential);

            EmisLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedEmisLandingOrchestrationServiceException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(someFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationServiceException))),
                        Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}
