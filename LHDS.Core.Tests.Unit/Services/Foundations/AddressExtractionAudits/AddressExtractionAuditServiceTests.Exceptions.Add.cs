using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit someAddressExtractionAudit = CreateRandomAddressExtractionAudit();
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
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(someAddressExtractionAudit);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAddressExtractionAuditAlreadyExsitsAndLogItAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            AddressExtractionAudit alreadyExistsAddressExtractionAudit = randomAddressExtractionAudit;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsAddressExtractionAuditException =
                new AlreadyExistsAddressExtractionAuditException(
                    message: "AddressExtractionAudit with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedAddressExtractionAuditDependencyValidationException =
                new AddressExtractionAuditDependencyValidationException(
                    message: "AddressExtractionAudit dependency validation occurred, please try again.",
                    innerException: alreadyExistsAddressExtractionAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(alreadyExistsAddressExtractionAudit);

            // then
            AddressExtractionAuditDependencyValidationException actualAddressExtractionAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            actualAddressExtractionAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
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

            var expectedAddressExtractionAuditValidationException =
                new AddressExtractionAuditDependencyValidationException(
                    message: "AddressExtractionAudit dependency validation occurred, please try again.",
                    innerException: invalidAddressExtractionAuditReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(someAddressExtractionAudit);

            // then
            AddressExtractionAuditDependencyValidationException actualAddressExtractionAuditDependencyValidationException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyValidationException>(
                    addAddressExtractionAuditTask.AsTask);

            actualAddressExtractionAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(someAddressExtractionAudit),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit someAddressExtractionAudit = CreateRandomAddressExtractionAudit();

            var databaseUpdateException =
                new DbUpdateException();

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
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(someAddressExtractionAudit);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit someAddressExtractionAudit = CreateRandomAddressExtractionAudit();
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
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.AddAddressExtractionAuditAsync(someAddressExtractionAudit);

            AddressExtractionAuditServiceException actualAddressExtractionAuditServiceException =
                await Assert.ThrowsAsync<AddressExtractionAuditServiceException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}