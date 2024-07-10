// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnBulkAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            SqlException sqlException = GetSqlException();

            var failedAddressStorageException = new FailedAddressStorageException(
                message: "Failed address storage error occurred, please contact support.",
                innerException: sqlException);

            var addressDependencyException = new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: failedAddressStorageException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process addresses in 1 of the batch(es) from {someFileName}",
                    addressDependencyException);

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed aggregate address service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask addAddressTask = this.addressService
                .BulkAddAddressesAsync(addresses: someAddresses, fileName: someFileName);

            AddressServiceException actualAddressDependencyException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    addAddressTask.AsTask);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    addressDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnBulkAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidAddressReferenceException =
                new InvalidAddressReferenceException(
                    message: "Invalid address reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var addressDependencyValidationException = new AddressDependencyValidationException(
                    message: "Address dependency validation occurred, please try again.",
                    innerException: invalidAddressReferenceException);

            var aggregateException =
                new AggregateException(
                    $"Unable to process addresses in 1 of the batch(es) from {someFileName}",
                    addressDependencyValidationException);

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed aggregate address service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask addAddressTask = this.addressService
                .BulkAddAddressesAsync(addresses: someAddresses, fileName: someFileName);

            // then
            AddressServiceException actualAddressServiceException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    addAddressTask.AsTask);

            actualAddressServiceException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnBulkAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };

            var databaseUpdateException =
                new DbUpdateException();

            var failedAddressStorageException =
                new FailedAddressStorageException(
                    message: "Failed address storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedAddressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: failedAddressStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask addAddressTask = this.addressService
                .BulkAddAddressesAsync(addresses: someAddresses, fileName: someFileName);

            AddressDependencyException actualAddressDependencyException =
                await Assert.ThrowsAsync<AddressDependencyException>(
                    addAddressTask.AsTask);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFileName = GetRandomString();
            List<Address> someAddresses = new List<Address> { CreateRandomAddress() };
            var serviceException = new Exception();

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask addAddressTask = this.addressService
                .BulkAddAddressesAsync(addresses: someAddresses, fileName: someFileName);

            AddressServiceException actualAddressServiceException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    addAddressTask.AsTask);

            // then
            actualAddressServiceException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}