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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
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
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressToUprnFileLog> addAddressToUprnFileLogTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyException actualAddressToUprnFileLogDependencyException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(
                    addAddressToUprnFileLogTask.AsTask);

            // then
            actualAddressToUprnFileLogDependencyException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
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
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAlreadyExistsAndLogItAsync()
        {
            // given
            AddressToUprnFileLog someAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
            string randomMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(randomMessage);

            var alreadyExistsAddressToUprnFileLogException =
                new AlreadyExistsAddressToUprnFileLogException(
                    message: "Address to UPRN file log with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedAddressToUprnFileLogDependencyValidationException =
                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation occurred, please try again.",
                    innerException: alreadyExistsAddressToUprnFileLogException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(GetRandomStringWithLengthOf(50));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<AddressToUprnFileLog> addAddressToUprnFileLogTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyValidationException>(
                    addAddressToUprnFileLogTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            AddressToUprnFileLog someAddressToUprnFileLog = CreateRandomAddressToUprnFileLog();
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
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(GetRandomStringWithLengthOf(50));

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<AddressToUprnFileLog> addAddressToUprnFileLogTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(
                    addAddressToUprnFileLogTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);
            AddressToUprnFileLog someAddressToUprnFileLog =
                CreateRandomAddressToUprnFileLog(randomDateTimeOffset, randomUserId);
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
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog))
                    .ReturnsAsync(someAddressToUprnFileLog);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressToUprnFileLog> addAddressToUprnFileLogTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(someAddressToUprnFileLog);

            AddressToUprnFileLogServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogServiceException>(
                    addAddressToUprnFileLogTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogServiceException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
