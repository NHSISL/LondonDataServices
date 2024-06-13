// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Download inputDownload = new Download { SubscriberCredential = inputSubscriberCredential };
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedDownloadServiceException =
                new FailedDownloadServiceException(
                    message: "Failed download service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDownloadServiceException =
                new DownloadServiceException(
                    message: "Download service error occurred, please contact support.",
                    innerException: failedDownloadServiceException);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> RetrieveListOfDocumentsToProcessTask =
                this.downloadService.RetrieveListOfDocumentsToProcessAsync(inputDownload);

            DownloadServiceException actualDownloadServiceException =
                await Assert.ThrowsAsync<DownloadServiceException>(RetrieveListOfDocumentsToProcessTask.AsTask);

            // then
            actualDownloadServiceException.Should()
                .BeEquivalentTo(expectedDownloadServiceException);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDownloadsToProcessAsync(inputDownload),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadServiceException))),
                        Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}