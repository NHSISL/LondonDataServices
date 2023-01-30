// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
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

            var invalidDownloadException =
                new InvalidDownloadException();

            invalidDownloadException.AddData(
                key: nameof(Document.FileName),
                values: "Text is required");

            var expectedDownloadValidationException =
                new DownloadValidationException(invalidDownloadException);

            // when
            ValueTask<Document> retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(invalidFileName);

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
                broker.GetDocumentByFileNameAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}