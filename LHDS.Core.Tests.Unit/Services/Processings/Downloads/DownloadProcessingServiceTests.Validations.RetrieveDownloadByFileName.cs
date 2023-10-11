// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfFileNameIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidFileName = invalidText;

            var invalidArgumentDownloadProcessingException =
                new InvalidArgumentDownloadProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDownloadProcessingException.AddData(
                key: nameof(Document.FileName),
                values: "Text is required");

            var expectedDownloadValidationException =
                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: invalidArgumentDownloadProcessingException);

            // when
            ValueTask<Document> retrieveDownloadByIdTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(invalidFileName);

            DownloadValidationException actualDownloadValidationException =
                await Assert.ThrowsAsync<DownloadValidationException>(
                    retrieveDownloadByIdTask.AsTask);

            // then
            actualDownloadValidationException.Should()
                .BeEquivalentTo(expectedDownloadValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.downloadServiceMock.Verify(broker =>
                broker.RetrieveDownloadByFileNameAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadServiceMock.VerifyNoOtherCalls();
        }
    }
}