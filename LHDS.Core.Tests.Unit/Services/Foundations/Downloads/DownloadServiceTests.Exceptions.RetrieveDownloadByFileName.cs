// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
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
            string someId = GetRandomMessage();
            var serviceException = new Exception();

            var failedDownloadServiceException =
                new FailedDownloadServiceException(serviceException);

            var expectedDownloadServiceException =
                new DownloadServiceException(
                    message: "Download service error occurred, contact support.", 
                    innerException: failedDownloadServiceException);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetDocumentByFileNameAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Document> retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByFileNameAsync(someId);

            DownloadServiceException actualDownloadServiceException =
                await Assert.ThrowsAsync<DownloadServiceException>(
                    retrieveDownloadByIdTask.AsTask);

            // then
            actualDownloadServiceException.Should()
                .BeEquivalentTo(expectedDownloadServiceException);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetDocumentByFileNameAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDownloadServiceException))),
                        Times.Once);

            this.downloadBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}