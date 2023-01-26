using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDownloadAlreadyExsitsAndLogItAsync()
        {
            // given
            Download randomDownload = CreateRandomDownload();
            Download alreadyExistsDownload = randomDownload;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsDownloadException =
                new AlreadyExistsDownloadException(duplicateKeyException);

            var expectedDownloadDependencyValidationException =
                new DownloadDependencyValidationException(alreadyExistsDownloadException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(alreadyExistsDownload);

            // then
            DownloadDependencyValidationException actualDownloadDependencyValidationException =
                await Assert.ThrowsAsync<DownloadDependencyValidationException>(
                    addDownloadTask.AsTask);

            actualDownloadDependencyValidationException.Should()
                .BeEquivalentTo(expectedDownloadDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(It.IsAny<Download>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Download someDownload = CreateRandomDownload();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDownloadReferenceException =
                new InvalidDownloadReferenceException(foreignKeyConstraintConflictException);

            var expectedDownloadValidationException =
                new DownloadDependencyValidationException(invalidDownloadReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Download> addDownloadTask =
                this.downloadService.AddDownloadAsync(someDownload);

            // then
            DownloadDependencyValidationException actualDownloadDependencyValidationException =
                await Assert.ThrowsAsync<DownloadDependencyValidationException>(
                    addDownloadTask.AsTask);

            actualDownloadDependencyValidationException.Should().BeEquivalentTo(expectedDownloadValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDownloadAsync(someDownload),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}