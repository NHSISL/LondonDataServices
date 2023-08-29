using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            SqlException sqlException = GetSqlException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, contact support.",
                    innerException: failedObjectColumnStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id))
                    .Throws(sqlException);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.RemoveObjectColumnByIdAsync(randomObjectColumn.Id);

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(
                    addObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteObjectColumnAsync(It.IsAny<ObjectColumn>()),
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
            Guid someObjectColumnId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedObjectColumnException =
                new LockedObjectColumnException(
                    message: "Locked objectColumn record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedObjectColumnDependencyValidationException =
                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.",
                    innerException: lockedObjectColumnException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ObjectColumn> removeObjectColumnByIdTask =
                this.objectColumnService.RemoveObjectColumnByIdAsync(someObjectColumnId);

            ObjectColumnDependencyValidationException actualObjectColumnDependencyValidationException =
                await Assert.ThrowsAsync<ObjectColumnDependencyValidationException>(
                    removeObjectColumnByIdTask.AsTask);

            // then
            actualObjectColumnDependencyValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}