using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            SqlException sqlException = GetSqlException();

            var failedAddressLoadingAuditStorageException =
                new FailedAddressLoadingAuditStorageException(
                    message: "Failed addressLoadingAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressLoadingAuditDependencyException =
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: failedAddressLoadingAuditStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(randomAddressLoadingAudit);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(randomAddressLoadingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidAddressLoadingAuditReferenceException =
                new InvalidAddressLoadingAuditReferenceException(
                    message: "Invalid addressLoadingAudit reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            AddressLoadingAuditDependencyValidationException expectedAddressLoadingAuditDependencyValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: invalidAddressLoadingAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(someAddressLoadingAudit);

            AddressLoadingAuditDependencyValidationException actualAddressLoadingAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(someAddressLoadingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAddressLoadingAuditDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(someAddressLoadingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            var databaseUpdateException = new DbUpdateException();

            var failedAddressLoadingAuditStorageException =
                new FailedAddressLoadingAuditStorageException(
                    message: "Failed addressLoadingAudit storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedAddressLoadingAuditDependencyException =
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: failedAddressLoadingAuditStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(randomAddressLoadingAudit);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(randomAddressLoadingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAddressLoadingAuditException =
                new LockedAddressLoadingAuditException(
                    message: "Locked addressLoadingAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAddressLoadingAuditDependencyValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: lockedAddressLoadingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(randomAddressLoadingAudit);

            AddressLoadingAuditDependencyValidationException actualAddressLoadingAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyValidationException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(randomAddressLoadingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            var serviceException = new Exception();

            var failedAddressLoadingAuditServiceException =
                new FailedAddressLoadingAuditServiceException(
                    message: "Failed addressLoadingAudit service occurred, please contact support", 
                    innerException: serviceException);

            var expectedAddressLoadingAuditServiceException =
                new AddressLoadingAuditServiceException(
                    message: "AddressLoadingAudit service error occurred, contact support.",
                    innerException: failedAddressLoadingAuditServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<AddressLoadingAudit> modifyAddressLoadingAuditTask =
                this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(randomAddressLoadingAudit);

            AddressLoadingAuditServiceException actualAddressLoadingAuditServiceException =
                await Assert.ThrowsAsync<AddressLoadingAuditServiceException>(
                    modifyAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(randomAddressLoadingAudit),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}