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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(It.IsAny<DecisionPoll>(), It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDecisionPollDependencyValidationException))),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    It.IsAny<DecisionPoll>(),
                    It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    It.IsAny<DecisionPoll>(),
                    It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyValidationException))),
                        Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    It.IsAny<DecisionPoll>(),
                    It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()))
                    .Throws(serviceException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollServiceException actualDecisionPollServiceException =
                await Assert.ThrowsAsync<DecisionPollServiceException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollServiceException.Should()
                .BeEquivalentTo(expectedDecisionPollServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(It.IsAny<DecisionPoll>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollServiceException))),
                        Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    It.IsAny<DecisionPoll>(),
                    It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
