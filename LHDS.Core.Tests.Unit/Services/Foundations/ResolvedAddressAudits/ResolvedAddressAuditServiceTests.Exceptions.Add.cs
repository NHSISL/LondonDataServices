// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressAuditStorageException =
                new FailedResolvedAddressAuditStorageException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressAuditDependencyException =
                new ResolvedAddressAuditDependencyException(
                    message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(someResolvedAddressAudit);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfResolvedAddressAuditAlreadyExsitsAndLogItAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            ResolvedAddressAudit alreadyExistsResolvedAddressAudit = randomResolvedAddressAudit;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsResolvedAddressAuditException =
                new AlreadyExistsResolvedAddressAuditException(
                    message: "ResolvedAddressAudit with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedResolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: alreadyExistsResolvedAddressAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(alreadyExistsResolvedAddressAudit);

            // then
            ResolvedAddressAuditDependencyValidationException actualResolvedAddressAuditDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyValidationException>(
                    addResolvedAddressAuditTask.AsTask);

            actualResolvedAddressAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidResolvedAddressAuditReferenceException =
                new InvalidResolvedAddressAuditReferenceException(
                    message: "Invalid resolvedAddressAudit reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: invalidResolvedAddressAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(someResolvedAddressAudit);

            // then
            ResolvedAddressAuditDependencyValidationException actualResolvedAddressAuditDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyValidationException>(
                    addResolvedAddressAuditTask.AsTask);

            actualResolvedAddressAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(someResolvedAddressAudit),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();

            var databaseUpdateException =
                new DbUpdateException();

            var failedResolvedAddressAuditStorageException =
                new FailedResolvedAddressAuditStorageException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedResolvedAddressAuditDependencyException =
                new ResolvedAddressAuditDependencyException(
                    message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(someResolvedAddressAudit);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit someResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            var serviceException = new Exception();

            var failedResolvedAddressAuditServiceException =
                new FailedResolvedAddressAuditServiceException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressAuditServiceException =
                new ResolvedAddressAuditServiceException(
                    message: "ResolvedAddressAudit service error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(someResolvedAddressAudit);

            ResolvedAddressAuditServiceException actualResolvedAddressAuditServiceException =
                await Assert.ThrowsAsync<ResolvedAddressAuditServiceException>(
                    addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}