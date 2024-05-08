// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAddresses())
                    .Throws(sqlException);

            // when
            Action retrieveAllAddressesAction = () =>
                this.addressService.RetrieveAllAddresses();

            AddressDependencyException actualAddressDependencyException =
                Assert.Throws<AddressDependencyException>(retrieveAllAddressesAction);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedAddressServiceException =
                new FailedAddressServiceException(
                    message: "Failed address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: failedAddressServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddresses())
                    .Throws(serviceException);

            // when
            Action retrieveAllAddressesAction = () =>
                this.addressService.RetrieveAllAddresses();

            AddressServiceException actualAddressServiceException =
                Assert.Throws<AddressServiceException>(retrieveAllAddressesAction);

            // then
            actualAddressServiceException.Should()
                .BeEquivalentTo(expectedAddressServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}