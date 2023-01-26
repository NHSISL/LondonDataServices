using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Downloads
{
    public partial class DownloadServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedDownloadStorageException =
                new FailedDownloadStorageException(sqlException);

            var expectedDownloadDependencyException =
                new DownloadDependencyException(failedDownloadStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDownloadByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Download> retrieveDownloadByIdTask =
                this.downloadService.RetrieveDownloadByIdAsync(someId);

            DownloadDependencyException actualDownloadDependencyException =
                await Assert.ThrowsAsync<DownloadDependencyException>(
                    retrieveDownloadByIdTask.AsTask);

            // then
            actualDownloadDependencyException.Should()
                .BeEquivalentTo(expectedDownloadDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDownloadByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDownloadDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}