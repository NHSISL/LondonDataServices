// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions;
using Moq;
using Xeptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DownloadDependancyValidationExceptions))]
        public async Task ShouldThrowDependancyValidationOnProcessIfDependancyValidationOccursAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // given
            var expectedDependancyException =
                new DownloadOrchestrationDependancyValidationException(
                    dependancyValidationException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationDependancyValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependancyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependancyException);

            this.downloadServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependancyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DownloadDependancyExceptions))]
        public async Task ShouldThrowDependancyExceptionOnProcessIfDependancyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // given
            var expectedDependancyException =
                new DownloadOrchestrationDependancyException(
                    dependancyException.InnerException as Xeption);

            this.downloadServiceMock.Setup(service =>
              service.RetrieveListOfDocumentsToProcessAsync())
                  .ThrowsAsync(dependancyException);

            // when
            ValueTask processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationDependancyException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationDependancyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependancyException);

            this.downloadServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependancyException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            var serviceException = new Exception();

            var failedDownloadOrchestrationServiceException =
               new FailedDownloadOrchestrationServiceException(serviceException);

            var expectedDownloadOrchestrationServiceException =
                new DownloadOrchestrationServiceException(failedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Setup(service =>
             service.RetrieveListOfDocumentsToProcessAsync())
                 .ThrowsAsync(serviceException);

            // when
            ValueTask processTask = this.downloadOrchestrationService.ProcessAsync();

            DownloadOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDownloadOrchestrationServiceException);

            this.downloadServiceMock.Verify(service =>
              service.RetrieveListOfDocumentsToProcessAsync(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDownloadOrchestrationServiceException))),
                       Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
