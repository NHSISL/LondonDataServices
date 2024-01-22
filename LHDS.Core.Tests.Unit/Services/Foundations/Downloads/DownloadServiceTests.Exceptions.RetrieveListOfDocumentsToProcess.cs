// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedDownloadServiceException =
                new FailedDownloadServiceException(
                    message: "Failed download service occurred, please contact support",
                    innerException: serviceException);

            var expectedDownloadServiceException =
                new DownloadServiceException(
                    message: "Download service error occurred, contact support.",
                    innerException: failedDownloadServiceException);

            this.downloadBrokerMock.Setup(broker =>
                broker.GetListOfDocumentsToProcessAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Document>> RetrieveListOfDocumentsToProcessTask =
                this.downloadService.RetrieveListOfDocumentsToProcessAsync();

            DownloadServiceException actualDownloadServiceException =
                await Assert.ThrowsAsync<DownloadServiceException>(RetrieveListOfDocumentsToProcessTask.AsTask);

            // then
            actualDownloadServiceException.Should()
                .BeEquivalentTo(expectedDownloadServiceException);

            this.downloadBrokerMock.Verify(broker =>
                broker.GetListOfDocumentsToProcessAsync(),
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