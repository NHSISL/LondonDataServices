// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAddressExtractionAuditStorageException =
                new FailedAddressExtractionAuditStorageException(
                    message: "Failed addressExtractionAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressExtractionAuditDependencyException =
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: failedAddressExtractionAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AddressExtractionAudit> retrieveAddressExtractionAuditByIdTask =
                this.addressExtractionAuditService.RetrieveAddressExtractionAuditByIdAsync(someId);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    retrieveAddressExtractionAuditByIdTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
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

            var failedAddressExtractionAuditServiceException =
                new FailedAddressExtractionAuditServiceException(
                    message: "Failed addressExtractionAudit service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressExtractionAuditServiceException =
                new AddressExtractionAuditServiceException(
                    message: "AddressExtractionAudit service error occurred, contact support.",
                    innerException: failedAddressExtractionAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AddressExtractionAudit> retrieveAddressExtractionAuditByIdTask =
                this.addressExtractionAuditService.RetrieveAddressExtractionAuditByIdAsync(someId);

            AddressExtractionAuditServiceException actualAddressExtractionAuditServiceException =
                await Assert.ThrowsAsync<AddressExtractionAuditServiceException>(
                    retrieveAddressExtractionAuditByIdTask.AsTask);

            // then
            actualAddressExtractionAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressExtractionAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}