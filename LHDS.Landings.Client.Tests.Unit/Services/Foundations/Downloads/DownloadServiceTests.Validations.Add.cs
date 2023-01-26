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
        public async Task ShouldThrowValidationExceptionOnAddIfDownloadIsNullAndLogItAsync()
        {
            // given
            Download nullDownload = null;

            var nullDownloadException =
                new NullDownloadException();

            var expectedDownloadValidationException =
                new DownloadValidationException(nullDownloadException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(nullDownload);

            DownloadValidationException actualDownloadValidationException =
                await Assert.ThrowsAsync<DownloadValidationException>(
                    addDownloadTask.AsTask);

            // then
            actualDownloadValidationException.Should().BeEquivalentTo(expectedDownloadValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}