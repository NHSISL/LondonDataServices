// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            SqlException sqlException = GetSqlException();

            var failedPdsAuditStorageException =
                new FailedPdsAuditStorageException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(
                    message: "PdsAudit dependency error occurred, please contact support.",
                    innerException: failedPdsAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPdsAuditReferenceException =
                new InvalidPdsAuditReferenceException(
                    message: "Invalid pdsAudit reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            PdsAuditDependencyValidationException expectedPdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: "PdsAudit dependency validation occurred, please try again.",
                    innerException: invalidPdsAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(somePdsAudit);

            PdsAuditDependencyValidationException actualPdsAuditDependencyValidationException =
                await Assert.ThrowsAsync<PdsAuditDependencyValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(somePdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPdsAuditDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(somePdsAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            var databaseUpdateException = new DbUpdateException();

            var failedPdsAuditStorageException =
                new FailedPdsAuditStorageException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(
                    message: "PdsAudit dependency error occurred, please contact support.",
                    innerException: failedPdsAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPdsAuditException =
                new LockedPdsAuditException(
                    message: "Locked pdsAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedPdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: "PdsAudit dependency validation occurred, please try again.",
                    innerException: lockedPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditDependencyValidationException actualPdsAuditDependencyValidationException =
                await Assert.ThrowsAsync<PdsAuditDependencyValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            var serviceException = new Exception();

            var failedPdsAuditServiceException =
                new FailedPdsAuditServiceException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsAuditServiceException =
                new PdsAuditServiceException(
                    message: "PdsAudit service error occurred, please contact support.",
                    innerException: failedPdsAuditServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditServiceException actualPdsAuditServiceException =
                await Assert.ThrowsAsync<PdsAuditServiceException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditServiceException.Should()
                .BeEquivalentTo(expectedPdsAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(randomPdsAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}