// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowDependencyValidationOnRetrieveFileIfDependencyValidationOccursAndLogItAsync(
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
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()))
                    .Throws(dependancyValidationException);

            // when
            ValueTask<byte[]> retrieveDownloadByFileNameTask = this.emisLandingOrchestrationService
                .RetrieveDownloadByFileNameAsync(
                    fileName: someFileName,
                    subscriberCredential: someSubscriberCredential);

            EmisLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationDependencyValidationException>(
                    retrieveDownloadByFileNameTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()),
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
