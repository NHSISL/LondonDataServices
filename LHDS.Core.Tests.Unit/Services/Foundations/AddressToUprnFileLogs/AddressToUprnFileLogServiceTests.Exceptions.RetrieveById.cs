// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
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
            ValueTask<AddressToUprnFileLog> retrieveByIdTask =
                this.addressToUprnFileLogService.RetrieveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogDependencyException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogDependencyException>(retrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
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
            ValueTask<AddressToUprnFileLog> retrieveByIdTask =
                this.addressToUprnFileLogService.RetrieveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogServiceException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogServiceException>(retrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
