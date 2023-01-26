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

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfDownloadIsNotFoundAndLogItAsync()
        {
            //given
            Guid someDownloadId = Guid.NewGuid();
            Download noDownload = null;

            var notFoundDownloadException =
                new NotFoundDownloadException(someDownloadId);

            var expectedDownloadValidationException =
                new DownloadValidationException(notFoundDownloadException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDownloadByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noDownload);

            //when
            ValueTask<Download> retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByIdAsync(someDownloadId);

            DownloadValidationException actualDownloadValidationException =
                await Assert.ThrowsAsync<DownloadValidationException>(
                    retrieveDownloadByIdTask.AsTask);

            //then
            actualDownloadValidationException.Should().BeEquivalentTo(expectedDownloadValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDownloadByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}