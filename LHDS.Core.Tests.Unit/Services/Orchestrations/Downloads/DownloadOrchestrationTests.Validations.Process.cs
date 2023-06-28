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

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    innerException: invalidArgumentDownloadOrchestrationException);

            // when
            ValueTask<string> DownloadTask =
                this.downloadOrchestrationService.ProcessAsync(invalidText);

            DownloadOrchestrationValidationException actualDownloadOrchestrationValidationException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualDownloadOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentsIfDownloadIsNullAsync()
        {
            // given

            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var notFoundDownloadOrchestrationException =
                new NotFoundDownloadOrchestrationException(fileName: inputFileName);

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    innerException: notFoundDownloadOrchestrationException);

            this.downloadServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(inputFileName))
                      .Returns(null);

            // when
            ValueTask<string> DownloadTask = this.downloadOrchestrationService.ProcessAsync(inputFileName);

            DownloadOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            this.downloadServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(inputFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
