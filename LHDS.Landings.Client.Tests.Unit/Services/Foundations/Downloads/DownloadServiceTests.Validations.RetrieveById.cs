using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidDownloadId = Guid.Empty;

            var invalidDownloadException =
                new InvalidDownloadException();

            invalidDownloadException.AddData(
                key: nameof(Download.Id),
                values: "Id is required");

            var expectedDownloadValidationException =
                new DownloadValidationException(invalidDownloadException);

            // when
            ValueTask<Download> retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByIdAsync(invalidDownloadId);

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

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDownloadByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}