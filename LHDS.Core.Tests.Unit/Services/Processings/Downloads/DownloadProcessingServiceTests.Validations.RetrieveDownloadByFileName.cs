// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Downloads
{
    public partial class DownloadProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveDownloadIfNullAndLogItAsync()
        {
            // given
            Download invalidDownload = null;

            var nullDownloadProcessingException =
                new NullDownloadProcessingException(
                    message: "Download is Null");

            var expectedDownloadProcessingValidationException =
                new DownloadProcessingValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: nullDownloadProcessingException);

            // when
            ValueTask retrieveDownloadByFileNameTask =
                this.downloadProcessingService.RetrieveDownloadByFileNameAsync(invalidDownload);

            DownloadProcessingValidationException actualDownloadProcessingValidationException =
                await Assert.ThrowsAsync<DownloadProcessingValidationException>(
                    retrieveDownloadByFileNameTask.AsTask);

            // then
            actualDownloadProcessingValidationException.Should()
                .BeEquivalentTo(expectedDownloadProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
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