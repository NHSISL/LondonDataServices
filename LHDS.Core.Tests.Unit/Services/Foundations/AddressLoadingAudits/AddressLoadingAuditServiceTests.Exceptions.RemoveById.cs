using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.RemoveAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    addAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(randomAddressLoadingAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
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
            Guid someAddressLoadingAuditId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedAddressLoadingAuditException =
                new LockedAddressLoadingAuditException(
                    message: "Locked addressLoadingAudit record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedAddressLoadingAuditDependencyValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: lockedAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<AddressLoadingAudit> removeAddressLoadingAuditByIdTask =
                this.addressLoadingAuditService.RemoveAddressLoadingAuditByIdAsync(someAddressLoadingAuditId);

            AddressLoadingAuditDependencyValidationException actualAddressLoadingAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyValidationException>(
                    removeAddressLoadingAuditByIdTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAddressLoadingAuditId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAddressLoadingAuditStorageException =
                new FailedAddressLoadingAuditStorageException(
                    message: "Failed addressLoadingAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressLoadingAuditDependencyException =
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: failedAddressLoadingAuditStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressLoadingAudit> deleteAddressLoadingAuditTask =
                this.addressLoadingAuditService.RemoveAddressLoadingAuditByIdAsync(someAddressLoadingAuditId);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    deleteAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}