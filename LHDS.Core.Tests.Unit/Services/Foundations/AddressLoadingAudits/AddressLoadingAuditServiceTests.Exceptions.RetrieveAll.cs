using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllAddressLoadingAudits())
                    .Throws(sqlException);

            // when
            Action retrieveAllAddressLoadingAuditsAction = () =>
                this.addressLoadingAuditService.RetrieveAllAddressLoadingAudits();

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                Assert.Throws<AddressLoadingAuditDependencyException>(retrieveAllAddressLoadingAuditsAction);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressLoadingAudits(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedAddressLoadingAuditServiceException =
                new FailedAddressLoadingAuditServiceException(
                    message: "Failed addressLoadingAudit service occurred, please contact support", 
                    innerException: serviceException);

            var expectedAddressLoadingAuditServiceException =
                new AddressLoadingAuditServiceException(
                    message: "AddressLoadingAudit service error occurred, contact support.",
                    innerException: failedAddressLoadingAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressLoadingAudits())
                    .Throws(serviceException);

            // when
            Action retrieveAllAddressLoadingAuditsAction = () =>
                this.addressLoadingAuditService.RetrieveAllAddressLoadingAudits();

            AddressLoadingAuditServiceException actualAddressLoadingAuditServiceException =
                Assert.Throws<AddressLoadingAuditServiceException>(retrieveAllAddressLoadingAuditsAction);

            // then
            actualAddressLoadingAuditServiceException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressLoadingAudits(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}