// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfDownloadIsNullAndLogItAsync()
        {
            // given
            Download invalidDownload = null;

            var nullDownloadException =
                new NullDownloadException(
                    message: "Download is null.");

            var expectedDownloadValidationException =
                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: nullDownloadException);

            // when
            ValueTask retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(invalidDownload);

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

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfSubscriberCredentialsIsNullAndLogItAsync()
        {
            // given
            Download invalidDownload = new Download
            {
                SubscriberCredential = null,
                Document = new Document { FileName = GetRandomString() }
            };

            var nullSubscriberCredentialException =
                new NullSubscriberCredentialException(
                    message: "SubscriberCredential is null.");

            var expectedDownloadValidationException =
                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialException);

            // when
            ValueTask retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(invalidDownload);

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

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfDocumentIsNullAndLogItAsync()
        {
            // given
            Download invalidDownload = new Download
            {
                SubscriberCredential = CreateRandomSubscriberCredential(),
                Document = null
            };

            var nullDocumentException =
                new NullDocumentException(
                    message: "Document is null.");

            var expectedDownloadValidationException =
                new DownloadValidationException(
                    message: "SubscriberCredential is null.",
                    innerException: nullDocumentException);

            // when
            ValueTask retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(invalidDownload);

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

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveByFileNameIfFileNameIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            Download invalidDownload = new Download
            {
                SubscriberCredential = CreateRandomSubscriberCredential(),
                Document = new Document
                {
                    FileName = invalidText
                }
            };

            var invalidDownloadException =
                new InvalidDownloadException(
                    message: "Invalid download. Please correct the errors and try again.");

            invalidDownloadException.AddData(
                key: nameof(Document.FileName),
                values: "Text is required");

            var expectedDownloadValidationException =
                new DownloadValidationException(
                    message: "Download validation errors occurred, please try again.",
                    innerException: invalidDownloadException);

            // when
            ValueTask retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(invalidDownload);

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

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}