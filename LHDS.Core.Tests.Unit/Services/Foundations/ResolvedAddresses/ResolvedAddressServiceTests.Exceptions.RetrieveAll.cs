// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolvedAddress storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "ResolvedAddress dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddresses())
                    .Throws(sqlException);

            // when
            Action retrieveAllResolvedAddressesAction = () =>
                this.resolvedAddressService.RetrieveAllResolvedAddresses();

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                Assert.Throws<ResolvedAddressDependencyException>(retrieveAllResolvedAddressesAction);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyException))),
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

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed resolvedAddress service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "ResolvedAddress service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddresses())
                    .Throws(serviceException);

            // when
            Action retrieveAllResolvedAddressesAction = () =>
                this.resolvedAddressService.RetrieveAllResolvedAddresses();

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                Assert.Throws<ResolvedAddressServiceException>(retrieveAllResolvedAddressesAction);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddresses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}