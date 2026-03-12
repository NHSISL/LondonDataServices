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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();
            SqlException sqlException = GetSqlException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(someSubscriberPractice);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfSubscriberPracticeAlreadyExsitsAndLogItAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice alreadyExistsSubscriberPractice = randomSubscriberPractice;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsSubscriberPracticeException =
                new AlreadyExistsSubscriberPracticeException(
                    message: "SubscriberPractice with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedSubscriberPracticeDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: alreadyExistsSubscriberPracticeException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(alreadyExistsSubscriberPractice))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(alreadyExistsSubscriberPractice);

            // then
            SubscriberPracticeDependencyValidationException actualSubscriberPracticeDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyValidationException>(
                    addSubscriberPracticeTask.AsTask);

            actualSubscriberPracticeDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(alreadyExistsSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: invalidSubscriberPracticeReferenceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(someSubscriberPractice);

            // then
            SubscriberPracticeDependencyValidationException actualSubscriberPracticeDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyValidationException>(
                    addSubscriberPracticeTask.AsTask);

            actualSubscriberPracticeDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(someSubscriberPractice),
                    Times.Never());

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();

            var databaseUpdateException =
                new DbUpdateException();

            var failedSubscriberPracticeStorageException =
                new FailedSubscriberPracticeStorageException(
                    message: "Failed subscriberPractice storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedSubscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: failedSubscriberPracticeStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(someSubscriberPractice);

            SubscriberPracticeDependencyException actualSubscriberPracticeDependencyException =
                await Assert.ThrowsAsync<SubscriberPracticeDependencyException>(
                    addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();
            var serviceException = new Exception();

            var failedSubscriberPracticeServiceException =
                new FailedSubscriberPracticeServiceException(
                    message: "Failed subscriberPractice service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: failedSubscriberPracticeServiceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(someSubscriberPractice);

            SubscriberPracticeServiceException actualSubscriberPracticeServiceException =
                await Assert.ThrowsAsync<SubscriberPracticeServiceException>(
                    addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeServiceException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeServiceException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}