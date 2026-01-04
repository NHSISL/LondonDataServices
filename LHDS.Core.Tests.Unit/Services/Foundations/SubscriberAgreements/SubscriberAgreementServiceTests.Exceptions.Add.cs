// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            SqlException sqlException = GetSqlException();

            var failedSubscriberAgreementStorageException =
                new FailedSubscriberAgreementStorageException(
                    message: "Failed subscriberAgreement storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSubscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: failedSubscriberAgreementStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(someSubscriberAgreement);

            SubscriberAgreementDependencyException actualSubscriberAgreementDependencyException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfSubscriberAgreementAlreadyExsitsAndLogItAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement alreadyExistsSubscriberAgreement = randomSubscriberAgreement;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsSubscriberAgreementException =
                new AlreadyExistsSubscriberAgreementException(
                    message: "SubscriberAgreement with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedSubscriberAgreementDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: "SubscriberAgreement dependency validation occurred, please try again.",
                    innerException: alreadyExistsSubscriberAgreementException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(alreadyExistsSubscriberAgreement);

            // then
            SubscriberAgreementDependencyValidationException actualSubscriberAgreementDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyValidationException>(
                    addSubscriberAgreementTask.AsTask);

            actualSubscriberAgreementDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyValidationException))),
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
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidSubscriberAgreementReferenceException =
                new InvalidSubscriberAgreementReferenceException(
                    message: "Invalid subscriberAgreement reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: "SubscriberAgreement dependency validation occurred, please try again.",
                    innerException: invalidSubscriberAgreementReferenceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(someSubscriberAgreement);

            // then
            SubscriberAgreementDependencyValidationException actualSubscriberAgreementDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyValidationException>(
                    addSubscriberAgreementTask.AsTask);

            actualSubscriberAgreementDependencyValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(someSubscriberAgreement),
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
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();

            var databaseUpdateException =
                new DbUpdateException();

            var failedSubscriberAgreementStorageException =
                new FailedSubscriberAgreementStorageException(
                    message: "Failed subscriberAgreement storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedSubscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: failedSubscriberAgreementStorageException);

            this.securityAuditBrokerMock.Setup(Brokers =>
                Brokers.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(someSubscriberAgreement);

            SubscriberAgreementDependencyException actualSubscriberAgreementDependencyException =
                await Assert.ThrowsAsync<SubscriberAgreementDependencyException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementDependencyException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementDependencyException))),
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
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            var serviceException = new Exception();

            var failedSubscriberAgreementServiceException =
                new FailedSubscriberAgreementServiceException(
                    message: "Failed subscriberAgreement service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberAgreementServiceException =
                new SubscriberAgreementServiceException(
                    message: "SubscriberAgreement service error occurred, please contact support.",
                    innerException: failedSubscriberAgreementServiceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(someSubscriberAgreement);

            SubscriberAgreementServiceException actualSubscriberAgreementServiceException =
                await Assert.ThrowsAsync<SubscriberAgreementServiceException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementServiceException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementServiceException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}