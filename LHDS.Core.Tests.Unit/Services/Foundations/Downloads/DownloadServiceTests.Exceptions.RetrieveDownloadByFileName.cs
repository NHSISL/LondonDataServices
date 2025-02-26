// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            Download someDownload = new Download
            {
                SubscriberCredential = someSubscriberCredential,
                Document = new Document { FileName = GetRandomString() }
            };

            string someId = GetRandomString();
            var serviceException = new Exception();

            var failedDownloadServiceException =
                new FailedDownloadServiceException(
                    message: "Failed download service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDownloadServiceException =
                new DownloadServiceException(
                    message: "Download service error occurred, please contact support.",
                    innerException: failedDownloadServiceException);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(someDownload);

            DownloadServiceException actualDownloadServiceException =
                await Assert.ThrowsAsync<DownloadServiceException>(
                    retrieveDownloadByIdTask.AsTask);

            // then
            actualDownloadServiceException.Should()
                .BeEquivalentTo(expectedDownloadServiceException);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDownloadByFileNameAsync(It.IsAny<Download>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDownloadServiceException))),
                        Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}