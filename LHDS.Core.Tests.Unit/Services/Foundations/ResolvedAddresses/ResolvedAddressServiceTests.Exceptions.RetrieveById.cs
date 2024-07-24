// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolved address storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResolvedAddress> retrieveResolvedAddressByIdTask =
                this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(someId);

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyException>(
                    retrieveResolvedAddressByIdTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedResolvedAddressServiceException =
                new FailedResolvedAddressServiceException(
                    message: "Failed resolved address service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: failedResolvedAddressServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddress> retrieveResolvedAddressByIdTask =
                this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(someId);

            ResolvedAddressServiceException actualResolvedAddressServiceException =
                await Assert.ThrowsAsync<ResolvedAddressServiceException>(
                    retrieveResolvedAddressByIdTask.AsTask);

            // then
            actualResolvedAddressServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}