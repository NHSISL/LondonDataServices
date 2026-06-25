// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressToUprnFileLog someAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            SqlException sqlException = GetSqlException();

            var failedAddressToUprnFileLogStorageException =
                new FailedAddressToUprnFileLogStorageException(
                    message: "Failed address to UPRN file log storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAddressToUprnFileLogDependencyException =
                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please contact support.",
                    innerException: failedAddressToUprnFileLogStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(GetRandomStringWithLengthOf(50));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(someAddressToUprnFileLog.Id),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(someAddressToUprnFileLog),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            AddressToUprnFileLog someAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(GetRandomString());

            var invalidAddressToUprnFileLogReferenceException =
                new InvalidAddressToUprnFileLogReferenceException(
                    message: "Invalid address to UPRN file log reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedAddressToUprnFileLogDependencyValidationException =
                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogReferenceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(GetRandomStringWithLengthOf(50));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyValidationException>(modifyTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfLockedAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog someAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAddressToUprnFileLogException =
                new LockedAddressToUprnFileLogException(
                    message: "Locked address to UPRN file log record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

            var expectedAddressToUprnFileLogDependencyValidationException =
                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation occurred, please try again.",
                    innerException: lockedAddressToUprnFileLogException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(someAddressToUprnFileLog.Id))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyValidationException>(modifyTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(someAddressToUprnFileLog.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog someAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            var dbUpdateException = new DbUpdateException();

            var failedAddressToUprnFileLogStorageException =
                new FailedAddressToUprnFileLogStorageException(
                    message: "Failed address to UPRN file log storage error occurred, please contact support.",
                    innerException: dbUpdateException);

            var expectedAddressToUprnFileLogDependencyException =
                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please contact support.",
                    innerException: failedAddressToUprnFileLogStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(someAddressToUprnFileLog.Id))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(modifyTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(someAddressToUprnFileLog.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            AddressToUprnFileLog someAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            var serviceException = new Exception(GetRandomString());

            var failedAddressToUprnFileLogServiceException =
                new FailedAddressToUprnFileLogServiceException(
                    message: "Failed address to UPRN file log service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressToUprnFileLogServiceException =
                new AddressToUprnFileLogServiceException(
                    message: "Address to UPRN file log service error occurred, please contact support.",
                    innerException: failedAddressToUprnFileLogServiceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(GetRandomStringWithLengthOf(50));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogServiceException>(modifyTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
