// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAddressExtractionAudits())
                    .Throws(sqlException);

            // when
            Action retrieveAllAddressExtractionAuditsAction = () =>
                this.addressExtractionAuditService.RetrieveAllAddressExtractionAudits();

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                Assert.Throws<AddressExtractionAuditDependencyException>(retrieveAllAddressExtractionAuditsAction);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressExtractionAudits(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedAddressExtractionAuditServiceException =
                new FailedAddressExtractionAuditServiceException(
                    message: "Failed addressExtractionAudit service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressExtractionAuditServiceException =
                new AddressExtractionAuditServiceException(
                    message: "AddressExtractionAudit service error occurred, contact support.",
                    innerException: failedAddressExtractionAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressExtractionAudits())
                    .Throws(serviceException);

            // when
            Action retrieveAllAddressExtractionAuditsAction = () =>
                this.addressExtractionAuditService.RetrieveAllAddressExtractionAudits();

            AddressExtractionAuditServiceException actualAddressExtractionAuditServiceException =
                Assert.Throws<AddressExtractionAuditServiceException>(retrieveAllAddressExtractionAuditsAction);

            // then
            actualAddressExtractionAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressExtractionAudits(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}