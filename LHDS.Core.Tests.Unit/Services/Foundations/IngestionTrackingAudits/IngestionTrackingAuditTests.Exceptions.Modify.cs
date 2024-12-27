// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingAuditStorageException =
                new FailedIngestionTrackingAuditStorageException(
                    message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingAuditDependencyException =
                new IngestionTrackingAuditDependencyException(
                    message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(sqlException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(randomIngestionTrackingAudit);

            IngestionTrackingAuditDependencyException actualIngestionTrackingAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(randomIngestionTrackingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidIngestionTrackingAuditReferenceException =
                new InvalidIngestionTrackingAuditReferenceException(
                    message: "Invalid IngestionTrackingAudit reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            IngestionTrackingAuditDependencyValidationException expectedIngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(someIngestionTrackingAudit);

            IngestionTrackingAuditDependencyValidationException actualIngestionTrackingAuditDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(someIngestionTrackingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedIngestionTrackingAuditDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(someIngestionTrackingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            var databaseUpdateException = new DbUpdateException();

            var failedIngestionTrackingAuditStorageException =
                new FailedIngestionTrackingAuditStorageException(
                    message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedIngestionTrackingAuditDependencyException =
                new IngestionTrackingAuditDependencyException(
                    message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(randomIngestionTrackingAudit);

            IngestionTrackingAuditDependencyException actualIngestionTrackingAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(randomIngestionTrackingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedIngestionTrackingAuditException =
                new LockedIngestionTrackingAuditException(
                    message: "Locked IngestionTrackingAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedIngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: lockedIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(randomIngestionTrackingAudit);

            IngestionTrackingAuditDependencyValidationException actualIngestionTrackingAuditDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(randomIngestionTrackingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            var serviceException = new Exception();

            var failedIngestionTrackingAuditServiceException =
                new FailedIngestionTrackingAuditServiceException(
                    message: "Failed IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingAuditServiceException =
                new IngestionTrackingAuditServiceException(
                    message: "IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Throws(serviceException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(randomIngestionTrackingAudit);

            IngestionTrackingAuditServiceException actualIngestionTrackingAuditServiceException =
                await Assert.ThrowsAsync<IngestionTrackingAuditServiceException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(randomIngestionTrackingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}