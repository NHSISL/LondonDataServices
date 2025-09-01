// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            SqlException sqlException = GetSqlException();

            var failedDecisionPollStorageException =
                new FailedDecisionPollStorageException(
                    message: "Failed decisionPoll storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDecisionPollDependencyException =
                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException: failedDecisionPollStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll someDecisionPoll = CreateRandomDecisionPoll();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidDecisionPollReferenceException =
                new InvalidDecisionPollReferenceException(
                    message: "Invalid decisionPoll reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            DecisionPollDependencyValidationException expectedDecisionPollDependencyValidationException =
                new DecisionPollDependencyValidationException(
                    message: "DecisionPoll dependency validation occurred, please try again.",
                    innerException: invalidDecisionPollReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(someDecisionPoll);

            DecisionPollDependencyValidationException actualDecisionPollDependencyValidationException =
                await Assert.ThrowsAsync<DecisionPollDependencyValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(someDecisionPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDecisionPollDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(someDecisionPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            var databaseUpdateException = new DbUpdateException();

            var failedDecisionPollStorageException =
                new FailedDecisionPollStorageException(
                    message: "Failed decisionPoll storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedDecisionPollDependencyException =
                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException: failedDecisionPollStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedDecisionPollException =
                new LockedDecisionPollException(
                    message: "Locked decisionPoll record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedDecisionPollDependencyValidationException =
                new DecisionPollDependencyValidationException(
                    message: "DecisionPoll dependency validation occurred, please try again.",
                    innerException: lockedDecisionPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollDependencyValidationException actualDecisionPollDependencyValidationException =
                await Assert.ThrowsAsync<DecisionPollDependencyValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            var serviceException = new Exception();

            var failedDecisionPollServiceException =
                new FailedDecisionPollServiceException(
                    message: "Failed decisionPoll service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecisionPollServiceException =
                new DecisionPollServiceException(
                    message: "DecisionPoll service error occurred, please contact support.",
                    innerException: failedDecisionPollServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollServiceException actualDecisionPollServiceException =
                await Assert.ThrowsAsync<DecisionPollServiceException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollServiceException.Should()
                .BeEquivalentTo(expectedDecisionPollServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(randomDecisionPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
