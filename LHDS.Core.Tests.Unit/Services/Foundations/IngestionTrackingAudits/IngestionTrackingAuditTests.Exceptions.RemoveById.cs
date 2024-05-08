// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<IngestionTrackingAudit> removeIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id);

            IngestionTrackingAuditDependencyException actualAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    removeIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(randomIngestionTrackingAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someIngestionTrackingAuditId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedIngestionTrackingAuditException =
                new LockedIngestionTrackingAuditException(
                    message: "Locked IngestionTrackingAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: lockedIngestionTrackingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<IngestionTrackingAudit> removeIngestionTrackingAuditByIdTask =
                this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(someIngestionTrackingAuditId);

            IngestionTrackingAuditDependencyValidationException actualAuditDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyValidationException>(
                    removeIngestionTrackingAuditByIdTask.AsTask);

            // then
            actualAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAuditDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAuditId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingAuditStorageException =
                new FailedIngestionTrackingAuditStorageException(
                    message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingAuditDependencyException =
                new IngestionTrackingAuditDependencyException(
                    message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IngestionTrackingAudit> deleteIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(someAuditId);

            IngestionTrackingAuditDependencyException actualAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    deleteIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someAuditId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedIngestionTrackingAuditServiceException =
                new FailedIngestionTrackingAuditServiceException(
                    message: "Failed IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAuditServiceException =
                new IngestionTrackingAuditServiceException(
                    message: "IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTrackingAudit> removeIngestionTrackingAuditByIdTask =
                this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(someAuditId);

            IngestionTrackingAuditServiceException actualAuditServiceException =
                await Assert.ThrowsAsync<IngestionTrackingAuditServiceException>(
                    removeIngestionTrackingAuditByIdTask.AsTask);

            // then
            actualAuditServiceException.Should()
                .BeEquivalentTo(expectedAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}