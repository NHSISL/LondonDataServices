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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit();
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
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(someAddressLoadingAudit);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    addAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAddressLoadingAuditAlreadyExsitsAndLogItAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            AddressLoadingAudit alreadyExistsAddressLoadingAudit = randomAddressLoadingAudit;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsAddressLoadingAuditException =
                new AlreadyExistsAddressLoadingAuditException(
                    message: "AddressLoadingAudit with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedAddressLoadingAuditDependencyValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: alreadyExistsAddressLoadingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(alreadyExistsAddressLoadingAudit);

            // then
            AddressLoadingAuditDependencyValidationException actualAddressLoadingAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyValidationException>(
                    addAddressLoadingAuditTask.AsTask);

            actualAddressLoadingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: invalidAddressLoadingAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(someAddressLoadingAudit);

            // then
            AddressLoadingAuditDependencyValidationException actualAddressLoadingAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyValidationException>(
                    addAddressLoadingAuditTask.AsTask);

            actualAddressLoadingAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(someAddressLoadingAudit),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit();

            var databaseUpdateException =
                new DbUpdateException();

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
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(someAddressLoadingAudit);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    addAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}