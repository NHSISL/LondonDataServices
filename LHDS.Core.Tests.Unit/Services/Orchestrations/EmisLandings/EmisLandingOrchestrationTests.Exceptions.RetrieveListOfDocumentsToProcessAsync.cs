// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}
