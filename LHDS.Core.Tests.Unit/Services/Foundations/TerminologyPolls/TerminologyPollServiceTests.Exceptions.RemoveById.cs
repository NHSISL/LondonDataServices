// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            SqlException sqlException = GetSqlException();

            var failedTerminologyPollStorageException =
                new FailedTerminologyPollStorageException(
                    message: "Failed terminologyPoll storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: failedTerminologyPollStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollService.RemoveTerminologyPollByIdAsync(randomTerminologyPoll.Id);

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    addTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someTerminologyPollId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedTerminologyPollException =
                new LockedTerminologyPollException(
                    message: "Locked terminologyPoll record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedTerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: lockedTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<TerminologyPoll> removeTerminologyPollByIdTask =
                this.terminologyPollService.RemoveTerminologyPollByIdAsync(someTerminologyPollId);

            TerminologyPollDependencyValidationException actualTerminologyPollDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyPollDependencyValidationException>(
                    removeTerminologyPollByIdTask.AsTask);

            // then
            actualTerminologyPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTerminologyPollId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedTerminologyPollStorageException =
                new FailedTerminologyPollStorageException(
                    message: "Failed terminologyPoll storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: failedTerminologyPollStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TerminologyPoll> deleteTerminologyPollTask =
                this.terminologyPollService.RemoveTerminologyPollByIdAsync(someTerminologyPollId);

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    deleteTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someTerminologyPollId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTerminologyPollServiceException =
                new FailedTerminologyPollServiceException(
                    message: "Failed terminologyPoll service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyPollServiceException =
                new TerminologyPollServiceException(
                    message: "TerminologyPoll service error occurred, please contact support.",
                    innerException: failedTerminologyPollServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TerminologyPoll> removeTerminologyPollByIdTask =
                this.terminologyPollService.RemoveTerminologyPollByIdAsync(someTerminologyPollId);

            TerminologyPollServiceException actualTerminologyPollServiceException =
                await Assert.ThrowsAsync<TerminologyPollServiceException>(
                    removeTerminologyPollByIdTask.AsTask);

            // then
            actualTerminologyPollServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}