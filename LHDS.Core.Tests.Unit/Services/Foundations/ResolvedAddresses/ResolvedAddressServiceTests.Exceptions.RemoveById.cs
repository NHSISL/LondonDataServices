using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolvedAddress storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "ResolvedAddress dependency error occurred, contact support.",
                    innerException: failedResolvedAddressStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(randomResolvedAddress.Id))
                    .Throws(sqlException);

            // when
            ValueTask<ResolvedAddress> addResolvedAddressTask =
                this.resolvedAddressService.RemoveResolvedAddressByIdAsync(randomResolvedAddress.Id);

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyException>(
                    addResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(randomResolvedAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
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
            Guid someResolvedAddressId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedResolvedAddressException =
                new LockedResolvedAddressException(
                    message: "Locked resolvedAddress record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedResolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "ResolvedAddress dependency validation occurred, please try again.",
                    innerException: lockedResolvedAddressException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ResolvedAddress> removeResolvedAddressByIdTask =
                this.resolvedAddressService.RemoveResolvedAddressByIdAsync(someResolvedAddressId);

            ResolvedAddressDependencyValidationException actualResolvedAddressDependencyValidationException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyValidationException>(
                    removeResolvedAddressByIdTask.AsTask);

            // then
            actualResolvedAddressDependencyValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}