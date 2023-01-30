// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
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

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
