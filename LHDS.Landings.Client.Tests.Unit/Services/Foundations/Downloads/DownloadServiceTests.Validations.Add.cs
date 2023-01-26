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
            actualDownloadValidationException.Should()
                .BeEquivalentTo(expectedDownloadValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDownloadIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDownload = new Download
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidDownloadException =
                new InvalidDownloadException();

            invalidDownloadException.AddData(
                key: nameof(Download.Id),
                values: "Id is required");

            //invalidDownloadException.AddData(
            //    key: nameof(Download.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Download model

            invalidDownloadException.AddData(
                key: nameof(Download.CreatedDate),
                values: "Date is required");

            invalidDownloadException.AddData(
                key: nameof(Download.CreatedByUserId),
                values: "Id is required");

            invalidDownloadException.AddData(
                key: nameof(Download.UpdatedDate),
                values: "Date is required");

            invalidDownloadException.AddData(
                key: nameof(Download.UpdatedByUserId),
                values: "Id is required");

            var expectedDownloadValidationException =
                new DownloadValidationException(invalidDownloadException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(invalidDownload);

            DownloadValidationException actualDownloadValidationException =
                await Assert.ThrowsAsync<DownloadValidationException>(
                    addDownloadTask.AsTask);

            // then
            actualDownloadValidationException.Should()
                .BeEquivalentTo(expectedDownloadValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Download randomDownload = CreateRandomDownload(randomDateTimeOffset);
            Download invalidDownload = randomDownload;

            invalidDownload.UpdatedDate =
                invalidDownload.CreatedDate.AddDays(randomNumber);

            var invalidDownloadException = new InvalidDownloadException();

            invalidDownloadException.AddData(
                key: nameof(Download.UpdatedDate),
                values: $"Date is not the same as {nameof(Download.CreatedDate)}");

            var expectedDownloadValidationException =
                new DownloadValidationException(invalidDownloadException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(invalidDownload);

            DownloadValidationException actualDownloadValidationException =
                await Assert.ThrowsAsync<DownloadValidationException>(
                    addDownloadTask.AsTask);

            // then
            actualDownloadValidationException.Should()
                .BeEquivalentTo(expectedDownloadValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}