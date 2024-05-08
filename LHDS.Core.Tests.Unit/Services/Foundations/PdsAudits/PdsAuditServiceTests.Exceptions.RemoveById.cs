// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(randomPdsAudit.Id);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(It.IsAny<PdsAudit>()),
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
            Guid somePdsAuditId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedPdsAuditException =
                new LockedPdsAuditException(
                    message: "Locked pdsAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedPdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: "PdsAudit dependency validation occurred, please try again.",
                    innerException: lockedPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PdsAudit> removePdsAuditByIdTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(somePdsAuditId);

            PdsAuditDependencyValidationException actualPdsAuditDependencyValidationException =
                await Assert.ThrowsAsync<PdsAuditDependencyValidationException>(
                    removePdsAuditByIdTask.AsTask);

            // then
            actualPdsAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePdsAuditId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPdsAuditStorageException =
                new FailedPdsAuditStorageException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(
                    message: "PdsAudit dependency error occurred, please contact support.",
                    innerException: failedPdsAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PdsAudit> deletePdsAuditTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(somePdsAuditId);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    deletePdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid somePdsAuditId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPdsAuditServiceException =
                new FailedPdsAuditServiceException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsAuditServiceException =
                new PdsAuditServiceException(
                    message: "PdsAudit service error occurred, please contact support.",
                    innerException: failedPdsAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PdsAudit> removePdsAuditByIdTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(somePdsAuditId);

            PdsAuditServiceException actualPdsAuditServiceException =
                await Assert.ThrowsAsync<PdsAuditServiceException>(
                    removePdsAuditByIdTask.AsTask);

            // then
            actualPdsAuditServiceException.Should()
                .BeEquivalentTo(expectedPdsAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}