using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Landings.Client.Models.Downloads.Exceptions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedDownloadStorageException(sqlException);

            var expectedDownloadDependencyException =
                new DownloadDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDownloads())
                    .Throws(sqlException);

            // when
            Action retrieveAllDownloadsAction = () =>
                this.downloadService.RetrieveAllDownloads();

            DownloadDependencyException actualDownloadDependencyException =
                Assert.Throws<DownloadDependencyException>(retrieveAllDownloadsAction);

            // then
            actualDownloadDependencyException.Should()
                .BeEquivalentTo(expectedDownloadDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDownloads(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDownloadDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedDownloadServiceException =
                new FailedDownloadServiceException(serviceException);

            var expectedDownloadServiceException =
                new DownloadServiceException(failedDownloadServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDownloads())
                    .Throws(serviceException);

            // when
            Action retrieveAllDownloadsAction = () =>
                this.downloadService.RetrieveAllDownloads();

            DownloadServiceException actualDownloadServiceException =
                Assert.Throws<DownloadServiceException>(retrieveAllDownloadsAction);

            // then
            actualDownloadServiceException.Should()
                .BeEquivalentTo(expectedDownloadServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDownloads(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}