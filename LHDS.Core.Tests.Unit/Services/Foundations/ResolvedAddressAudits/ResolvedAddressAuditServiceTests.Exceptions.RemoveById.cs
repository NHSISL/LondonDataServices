// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(randomResolvedAddressAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
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
            Guid someResolvedAddressAuditId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedResolvedAddressAuditException =
                new LockedResolvedAddressAuditException(
                    message: "Locked resolvedAddressAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedResolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: lockedResolvedAddressAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ResolvedAddressAudit> removeResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(someResolvedAddressAuditId);

            ResolvedAddressAuditDependencyValidationException actualResolvedAddressAuditDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyValidationException>(
                    removeResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someResolvedAddressAuditId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressAuditStorageException =
                new FailedResolvedAddressAuditStorageException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressAuditDependencyException =
                new ResolvedAddressAuditDependencyException(
                    message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResolvedAddressAudit> deleteResolvedAddressAuditTask =
                this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(someResolvedAddressAuditId);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    deleteResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someResolvedAddressAuditId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedResolvedAddressAuditServiceException =
                new FailedResolvedAddressAuditServiceException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressAuditServiceException =
                new ResolvedAddressAuditServiceException(
                    message: "ResolvedAddressAudit service error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddressAudit> removeResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(someResolvedAddressAuditId);

            ResolvedAddressAuditServiceException actualResolvedAddressAuditServiceException =
                await Assert.ThrowsAsync<ResolvedAddressAuditServiceException>(
                    removeResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}