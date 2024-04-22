// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAddressLoadingAuditStorageException =
                new FailedAddressLoadingAuditStorageException(
                    message: "Failed addressLoadingAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressLoadingAuditDependencyException =
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: failedAddressLoadingAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressLoadingAudit> retrieveAddressLoadingAuditByIdTask =
                this.addressLoadingAuditService.RetrieveAddressLoadingAuditByIdAsync(someId);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    retrieveAddressLoadingAuditByIdTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
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

            var failedAddressLoadingAuditServiceException =
                new FailedAddressLoadingAuditServiceException(
                    message: "Failed addressLoadingAudit service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressLoadingAuditServiceException =
                new AddressLoadingAuditServiceException(
                    message: "AddressLoadingAudit service error occurred, contact support.",
                    innerException: failedAddressLoadingAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressLoadingAudit> retrieveAddressLoadingAuditByIdTask =
                this.addressLoadingAuditService.RetrieveAddressLoadingAuditByIdAsync(someId);

            AddressLoadingAuditServiceException actualAddressLoadingAuditServiceException =
                await Assert.ThrowsAsync<AddressLoadingAuditServiceException>(
                    retrieveAddressLoadingAuditByIdTask.AsTask);

            // then
            actualAddressLoadingAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressLoadingAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}