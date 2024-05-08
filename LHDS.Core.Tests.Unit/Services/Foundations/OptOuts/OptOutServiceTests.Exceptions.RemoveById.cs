// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
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

            var failedOptOutStorageException = new FailedOptOutStorageException(
                message: "Failed optOut storage error occurred, please contact support.",
                innerException: sqlException);

            var expectedOptOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: failedOptOutStorageException);

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

            var lockedOptOutException = new LockedOptOutException(
                message: "Locked optOut record exception, please try again later",
                innerException: databaseUpdateConcurrencyException);

            var expectedOptOutDependencyValidationException = new OptOutDependencyValidationException(
                message: "OptOut dependency validation occurred, please try again.",
                innerException: lockedOptOutException);

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

            var failedOptOutStorageException = new FailedOptOutStorageException(
                message: "Failed optOut storage error occurred, please contact support.",
                innerException: sqlException);

            var expectedOptOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: failedOptOutStorageException);

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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someOptOutId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedOptOutServiceException = new FailedOptOutServiceException(
                message: "Failed optOut service error occurred, please contact support.",
                innerException: serviceException);

            var expectedOptOutServiceException = new OptOutServiceException(
                message: "OptOut service error occurred, please contact support.",
                innerException: failedOptOutServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OptOut> removeOptOutByIdTask =
                this.optOutService.RemoveOptOutByIdAsync(someOptOutId);

            OptOutServiceException actualOptOutServiceException =
                await Assert.ThrowsAsync<OptOutServiceException>(
                    removeOptOutByIdTask.AsTask);

            // then
            actualOptOutServiceException.Should()
                .BeEquivalentTo(expectedOptOutServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}