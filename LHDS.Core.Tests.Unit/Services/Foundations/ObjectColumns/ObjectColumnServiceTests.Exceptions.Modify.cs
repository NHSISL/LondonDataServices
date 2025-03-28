// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            SqlException sqlException = GetSqlException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: failedObjectColumnStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn someObjectColumn = CreateRandomObjectColumn();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidObjectColumnReferenceException =
                new InvalidObjectColumnReferenceException(
                    message: "Invalid objectColumn reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            ObjectColumnDependencyValidationException expectedObjectColumnDependencyValidationException =
                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.",
                    innerException: invalidObjectColumnReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(someObjectColumn);

            ObjectColumnDependencyValidationException actualObjectColumnDependencyValidationException =
                await Assert.ThrowsAsync<ObjectColumnDependencyValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(someObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedObjectColumnDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(someObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            var databaseUpdateException = new DbUpdateException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: failedObjectColumnStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedObjectColumnException =
                new LockedObjectColumnException(
                    message: "Locked objectColumn record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedObjectColumnDependencyValidationException =
                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.",
                    innerException: lockedObjectColumnException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnDependencyValidationException actualObjectColumnDependencyValidationException =
                await Assert.ThrowsAsync<ObjectColumnDependencyValidationException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyValidationException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            var serviceException = new Exception();

            var failedObjectColumnServiceException =
                new FailedObjectColumnServiceException(
                    message: "Failed objectColumn service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedObjectColumnServiceException =
                new ObjectColumnServiceException(
                    message: "ObjectColumn service error occurred, please contact support.",
                    innerException: failedObjectColumnServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnServiceException actualObjectColumnServiceException =
                await Assert.ThrowsAsync<ObjectColumnServiceException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnServiceException.Should()
                .BeEquivalentTo(expectedObjectColumnServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}