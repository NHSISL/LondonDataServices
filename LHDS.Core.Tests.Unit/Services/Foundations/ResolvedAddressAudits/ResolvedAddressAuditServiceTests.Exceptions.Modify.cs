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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
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
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(randomResolvedAddressAudit);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(randomResolvedAddressAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
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

            ResolvedAddressAuditDependencyValidationException expectedResolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: invalidResolvedAddressAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(someResolvedAddressAudit);

            ResolvedAddressAuditDependencyValidationException actualResolvedAddressAuditDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(someResolvedAddressAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedResolvedAddressAuditDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(someResolvedAddressAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            var databaseUpdateException = new DbUpdateException();

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
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(randomResolvedAddressAudit);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(randomResolvedAddressAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedResolvedAddressAuditException =
                new LockedResolvedAddressAuditException(
                    message: "Locked resolvedAddressAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedResolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: lockedResolvedAddressAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(randomResolvedAddressAudit);

            ResolvedAddressAuditDependencyValidationException actualResolvedAddressAuditDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(randomResolvedAddressAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
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
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(randomResolvedAddressAudit);

            ResolvedAddressAuditServiceException actualResolvedAddressAuditServiceException =
                await Assert.ThrowsAsync<ResolvedAddressAuditServiceException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(randomResolvedAddressAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}