// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByPostCodeIfSqlErrorOccursAndLogItAsync()
        {
            // given
            string someString = GetRandomString();
            SqlException sqlException = GetSqlException();

            var failedAddressStorageException =
                new FailedAddressStorageException(
                    message: "Failed address storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedAddressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: failedAddressStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<List<Address>> retrieveAddressesByPostCodeTask =
                this.addressService.RetrieveAddressesByPostCodeAsync(someString);

            AddressDependencyException actualAddressDependencyException =
                await Assert.ThrowsAsync<AddressDependencyException>(
                    retrieveAddressesByPostCodeTask.AsTask);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedAddressDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByPostCodeIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someString = GetRandomString();
            var serviceException = new Exception();

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Address>> retrieveAddressesByPostCodeTask =
                this.addressService.RetrieveAddressesByPostCodeAsync(someString);

            AddressServiceException actualAddressServiceException =
                await Assert.ThrowsAsync<AddressServiceException>(
                    retrieveAddressesByPostCodeTask.AsTask);

            // then
            actualAddressServiceException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAddressServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}