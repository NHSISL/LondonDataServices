using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OptOut randomOptOut = CreateRandomOptOut();
            SqlException sqlException = GetSqlException();

            var failedOptOutStorageException =
                new FailedOptOutStorageException(sqlException);

            var expectedOptOutDependencyException =
                new OptOutDependencyException(failedOptOutStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(randomOptOut.Id))
                    .Throws(sqlException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.RemoveOptOutByIdAsync(randomOptOut.Id);

            OptOutDependencyException actualOptOutDependencyException =
                await Assert.ThrowsAsync<OptOutDependencyException>(
                    addOptOutTask.AsTask);

            // then
            actualOptOutDependencyException.Should()
                .BeEquivalentTo(expectedOptOutDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(randomOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOptOutDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someOptOutId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedOptOutException =
                new LockedOptOutException(databaseUpdateConcurrencyException);

            var expectedOptOutDependencyValidationException =
                new OptOutDependencyValidationException(lockedOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<OptOut> removeOptOutByIdTask =
                this.optOutService.RemoveOptOutByIdAsync(someOptOutId);

            OptOutDependencyValidationException actualOptOutDependencyValidationException =
                await Assert.ThrowsAsync<OptOutDependencyValidationException>(
                    removeOptOutByIdTask.AsTask);

            // then
            actualOptOutDependencyValidationException.Should()
                .BeEquivalentTo(expectedOptOutDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someOptOutId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedOptOutStorageException =
                new FailedOptOutStorageException(sqlException);

            var expectedOptOutDependencyException =
                new OptOutDependencyException(failedOptOutStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OptOut> deleteOptOutTask =
                this.optOutService.RemoveOptOutByIdAsync(someOptOutId);

            OptOutDependencyException actualOptOutDependencyException =
                await Assert.ThrowsAsync<OptOutDependencyException>(
                    deleteOptOutTask.AsTask);

            // then
            actualOptOutDependencyException.Should()
                .BeEquivalentTo(expectedOptOutDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOptOutDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}