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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll someDecisionPoll = CreateRandomDecisionPoll();
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
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(someDecisionPoll);

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    addDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDecisionPollAlreadyExistsAndLogItAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            DecisionPoll alreadyExistsDecisionPoll = randomDecisionPoll;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsDecisionPollException =
                new AlreadyExistsDecisionPollException(
                    message: "DecisionPoll with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedDecisionPollDependencyValidationException =
                new DecisionPollDependencyValidationException(
                    message: "DecisionPoll dependency validation occurred, please try again.",
                    innerException: alreadyExistsDecisionPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(alreadyExistsDecisionPoll);

            // then
            DecisionPollDependencyValidationException actualDecisionPollDependencyValidationException =
                await Assert.ThrowsAsync<DecisionPollDependencyValidationException>(
                    addDecisionPollTask.AsTask);

            actualDecisionPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedDecisionPollValidationException =
                new DecisionPollDependencyValidationException(
                    message: "DecisionPoll dependency validation occurred, please try again.",
                    innerException: invalidDecisionPollReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(someDecisionPoll);

            // then
            DecisionPollDependencyValidationException actualDecisionPollDependencyValidationException =
                await Assert.ThrowsAsync<DecisionPollDependencyValidationException>(
                    addDecisionPollTask.AsTask);

            actualDecisionPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(someDecisionPoll),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll someDecisionPoll = CreateRandomDecisionPoll();

            var databaseUpdateException =
                new DbUpdateException();

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
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(someDecisionPoll);

            DecisionPollDependencyException actualDecisionPollDependencyException =
                await Assert.ThrowsAsync<DecisionPollDependencyException>(
                    addDecisionPollTask.AsTask);

            // then
            actualDecisionPollDependencyException.Should()
                .BeEquivalentTo(expectedDecisionPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DecisionPoll someDecisionPoll = CreateRandomDecisionPoll();
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
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(someDecisionPoll);

            DecisionPollServiceException actualDecisionPollServiceException =
                await Assert.ThrowsAsync<DecisionPollServiceException>(
                    addDecisionPollTask.AsTask);

            // then
            actualDecisionPollServiceException.Should()
                .BeEquivalentTo(expectedDecisionPollServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
