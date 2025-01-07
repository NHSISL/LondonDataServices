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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
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
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(someIngestionTrackingAudit);

            IngestionTrackingAuditDependencyException actualIngestionTrackingAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAuditAlreadyExsitsAndLogItAsync()
        {
            // given
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit alreadyExistsIngestionTrackingAudit = randomIngestionTrackingAudit;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsIngestionTrackingAuditException =
                new AlreadyExistsIngestionTrackingAuditException(
                    message: "IngestionTrackingAudit with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedIngestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: alreadyExistsIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(alreadyExistsIngestionTrackingAudit);

            // then
            IngestionTrackingAuditDependencyValidationException actualAuditDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            actualAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(someIngestionTrackingAudit);

            // then
            IngestionTrackingAuditDependencyValidationException actualIngestionTrackingAuditDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            actualIngestionTrackingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(someIngestionTrackingAudit),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();

            var databaseUpdateException =
                new DbUpdateException();

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
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(someIngestionTrackingAudit);

            IngestionTrackingAuditDependencyException actualIngestionTrackingAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
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
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(someIngestionTrackingAudit);

            IngestionTrackingAuditServiceException actualIngestionTrackingAuditServiceException =
                await Assert.ThrowsAsync<IngestionTrackingAuditServiceException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}