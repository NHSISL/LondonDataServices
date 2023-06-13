// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnDownloadIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            var invalidArgumentDownloadOrchestrationException =
                new InvalidArgumentDownloadOrchestrationException();

            invalidArgumentDownloadOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedDownloadOrchestrationFileNameValidationException =
                new DownloadOrchestrationValidationException(
                    innerException: invalidArgumentDownloadOrchestrationException);

            // when
            ValueTask<string> DownloadTask =
                this.downloadOrchestrationService.ProcessAsync(invalidText);

            DownloadOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationFileNameValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationFileNameValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
