// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            TerminologyPoll someTerminologyPoll = CreateRandomTerminologyPoll();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTerminologyPollReferenceException =
                new InvalidTerminologyPollReferenceException(
                    message: "Invalid terminologyPoll reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            TerminologyPollDependencyValidationException expectedTerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: invalidTerminologyPollReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(someTerminologyPoll);

            TerminologyPollDependencyValidationException actualTerminologyPollDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyPollDependencyValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(someTerminologyPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTerminologyPollDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(someTerminologyPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            var databaseUpdateException = new DbUpdateException();

            var failedTerminologyPollStorageException =
                new FailedTerminologyPollStorageException(
                    message: "Failed terminologyPoll storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedTerminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: failedTerminologyPollStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollDependencyException actualTerminologyPollDependencyException =
                await Assert.ThrowsAsync<TerminologyPollDependencyException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTerminologyPollException =
                new LockedTerminologyPollException(
                    message: "Locked terminologyPoll record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedTerminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: lockedTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollDependencyValidationException actualTerminologyPollDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyPollDependencyValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            var serviceException = new Exception();

            var failedTerminologyPollServiceException =
                new FailedTerminologyPollServiceException(
                    message: "Failed terminologyPoll service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyPollServiceException =
                new TerminologyPollServiceException(
                    message: "TerminologyPoll service error occurred, please contact support.",
                    innerException: failedTerminologyPollServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollServiceException actualTerminologyPollServiceException =
                await Assert.ThrowsAsync<TerminologyPollServiceException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollServiceException.Should()
                .BeEquivalentTo(expectedTerminologyPollServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(randomTerminologyPoll.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}