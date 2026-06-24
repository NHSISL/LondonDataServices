// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAddressToUprnFileLogStorageException =
                new FailedAddressToUprnFileLogStorageException(
                    message: "Failed address to UPRN file log storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAddressToUprnFileLogDependencyException =
                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please contact support.",
                    innerException: failedAddressToUprnFileLogStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressToUprnFileLog> removeByIdTask =
                this.addressToUprnFileLogService.RemoveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(removeByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfLockedAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAddressToUprnFileLogException =
                new LockedAddressToUprnFileLogException(
                    message: "Locked address to UPRN file log record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

            var expectedAddressToUprnFileLogDependencyValidationException =
                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation occurred, please try again.",
                    innerException: lockedAddressToUprnFileLogException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<AddressToUprnFileLog> removeByIdTask =
                this.addressToUprnFileLogService.RemoveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyValidationException>(removeByIdTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception(GetRandomString());

            var failedAddressToUprnFileLogServiceException =
                new FailedAddressToUprnFileLogServiceException(
                    message: "Failed address to UPRN file log service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressToUprnFileLogServiceException =
                new AddressToUprnFileLogServiceException(
                    message: "Address to UPRN file log service error occurred, please contact support.",
                    innerException: failedAddressToUprnFileLogServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressToUprnFileLog> removeByIdTask =
                this.addressToUprnFileLogService.RemoveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogServiceException>(removeByIdTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressToUprnFileLogServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
