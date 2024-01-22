// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            SqlException sqlException = GetSqlException();

            var failedAddressExtractionAuditStorageException =
                new FailedAddressExtractionAuditStorageException(
                    message: "Failed addressExtractionAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressExtractionAuditDependencyException =
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: failedAddressExtractionAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(randomAddressExtractionAudit);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(randomAddressExtractionAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit someAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidAddressExtractionAuditReferenceException =
                new InvalidAddressExtractionAuditReferenceException(
                    message: "Invalid addressExtractionAudit reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            AddressExtractionAuditDependencyValidationException expectedAddressExtractionAuditDependencyValidationException =
                new AddressExtractionAuditDependencyValidationException(
                    message: "AddressExtractionAudit dependency validation occurred, please try again.",
                    innerException: invalidAddressExtractionAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(someAddressExtractionAudit);

            AddressExtractionAuditDependencyValidationException actualAddressExtractionAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyValidationException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(someAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAddressExtractionAuditDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(someAddressExtractionAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            var databaseUpdateException = new DbUpdateException();

            var failedAddressExtractionAuditStorageException =
                new FailedAddressExtractionAuditStorageException(
                    message: "Failed addressExtractionAudit storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedAddressExtractionAuditDependencyException =
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: failedAddressExtractionAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(randomAddressExtractionAudit);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(randomAddressExtractionAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAddressExtractionAuditException =
                new LockedAddressExtractionAuditException(
                    message: "Locked addressExtractionAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAddressExtractionAuditDependencyValidationException =
                new AddressExtractionAuditDependencyValidationException(
                    message: "AddressExtractionAudit dependency validation occurred, please try again.",
                    innerException: lockedAddressExtractionAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(randomAddressExtractionAudit);

            AddressExtractionAuditDependencyValidationException actualAddressExtractionAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyValidationException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(randomAddressExtractionAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            var serviceException = new Exception();

            var failedAddressExtractionAuditServiceException =
                new FailedAddressExtractionAuditServiceException(
                    message: "Failed addressExtractionAudit service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressExtractionAuditServiceException =
                new AddressExtractionAuditServiceException(
                    message: "AddressExtractionAudit service error occurred, contact support.",
                    innerException: failedAddressExtractionAuditServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<AddressExtractionAudit> modifyAddressExtractionAuditTask =
                this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(randomAddressExtractionAudit);

            AddressExtractionAuditServiceException actualAddressExtractionAuditServiceException =
                await Assert.ThrowsAsync<AddressExtractionAuditServiceException>(
                    modifyAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(randomAddressExtractionAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}