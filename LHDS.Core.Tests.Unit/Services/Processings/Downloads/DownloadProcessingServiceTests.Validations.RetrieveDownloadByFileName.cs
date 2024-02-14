// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        // TODO: 

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfFileNameIsInvalidAndLogItAsync()
        {
            // given
            Download invalidDownload = null;

            var invalidArgumentDownloadProcessingException =
                new InvalidArgumentDownloadProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentDownloadProcessingException.AddData(
                key: nameof(Document.FileName),
                values: "Text is required");

            var expectedDownloadProcessingValidationException =
                new DownloadProcessingValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: invalidArgumentDownloadProcessingException);

            // when
            ValueTask<Download> retrieveDownloadByFileNameTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(invalidDownload);

            DownloadProcessingValidationException actualDownloadProcessingValidationException =
                await Assert.ThrowsAsync<DownloadProcessingValidationException>(
                    retrieveDownloadByFileNameTask.AsTask);

            // then
            actualDownloadProcessingValidationException.Should()
                .BeEquivalentTo(expectedDownloadProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadProcessingValidationException))),
                        Times.Once);

            this.downloadServiceMock.Verify(broker =>
                broker.RetrieveDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadServiceMock.VerifyNoOtherCalls();
        }
    }
}