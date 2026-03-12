// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(randomSubscriberPractice.Id);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(randomSubscriberPractice.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
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
            Guid someSubscriberPracticeId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedSubscriberPracticeException =
                new LockedSubscriberPracticeException(
                    message: "Locked subscriberPractice record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedSubscriberPracticeDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: lockedSubscriberPracticeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<SubscriberPractice> removeSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(someSubscriberPracticeId);

            SubscriberPracticeDependencyValidationException actualSubscriberPracticeDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyValidationException>(
                    removeSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSubscriberPracticeId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberPractice> deleteSubscriberPracticeTask =
                this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(someSubscriberPracticeId);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    deleteSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someSubscriberPracticeId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSubscriberPracticeServiceException =
                new FailedSubscriberPracticeServiceException(
                    message: "Failed subscriberPractice service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: failedSubscriberPracticeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberPractice> removeSubscriberPracticeByIdTask =
                this.subscriberPracticeService.RemoveSubscriberPracticeByIdAsync(someSubscriberPracticeId);

            SubscriberPracticeServiceException actualSubscriberPracticeServiceException =
                await Assert.ThrowsAsync<SubscriberPracticeServiceException>(
                    removeSubscriberPracticeByIdTask.AsTask);

            // then
            actualSubscriberPracticeServiceException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}