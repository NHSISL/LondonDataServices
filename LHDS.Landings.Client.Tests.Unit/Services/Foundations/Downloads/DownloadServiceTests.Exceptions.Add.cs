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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Download someDownload = CreateRandomDownload();
            SqlException sqlException = GetSqlException();

            var failedDownloadStorageException =
                new FailedDownloadStorageException(sqlException);

            var expectedDownloadDependencyException =
                new DownloadDependencyException(failedDownloadStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(someDownload);

            DownloadDependencyException actualDownloadDependencyException =
                await Assert.ThrowsAsync<DownloadDependencyException>(
                    addDownloadTask.AsTask);

            // then
            actualDownloadDependencyException.Should()
                .BeEquivalentTo(expectedDownloadDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDownloadDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}