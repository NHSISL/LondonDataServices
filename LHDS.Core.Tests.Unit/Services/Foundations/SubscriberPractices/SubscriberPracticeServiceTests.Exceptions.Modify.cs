// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SqlException sqlException = GetSqlException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(randomSubscriberPractice);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(randomSubscriberPractice),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSubscriberPracticeReferenceException =
                new InvalidSubscriberPracticeReferenceException(
                    message: "Invalid subscriberPractice reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            SubscriberPracticeDependencyValidationException expectedSubscriberPracticeDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: invalidSubscriberPracticeReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(someSubscriberPractice);

            SubscriberPracticeDependencyValidationException actualSubscriberPracticeDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(someSubscriberPractice.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedSubscriberPracticeDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(someSubscriberPractice),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            var databaseUpdateException = new DbUpdateException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(randomSubscriberPractice);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(randomSubscriberPractice),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSubscriberPracticeException =
                new LockedSubscriberPracticeException(
                    message: "Locked subscriberPractice record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedSubscriberPracticeDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: lockedSubscriberPracticeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(randomSubscriberPractice);

            SubscriberPracticeDependencyValidationException actualSubscriberPracticeDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(randomSubscriberPractice),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            var serviceException = new Exception();

            var failedSubscriberPracticeServiceException =
                new FailedSubscriberPracticeServiceException(
                    message: "Failed subscriberPractice service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: failedSubscriberPracticeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(randomSubscriberPractice);

            SubscriberPracticeServiceException actualSubscriberPracticeServiceException =
                await Assert.ThrowsAsync<SubscriberPracticeServiceException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeServiceException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(randomSubscriberPractice),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}