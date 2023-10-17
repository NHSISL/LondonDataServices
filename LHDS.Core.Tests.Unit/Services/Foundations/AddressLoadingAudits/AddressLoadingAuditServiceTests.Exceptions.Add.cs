using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            SqlException sqlException = GetSqlException();

            var failedAddressLoadingAuditStorageException =
                new FailedAddressLoadingAuditStorageException(
                    message: "Failed addressLoadingAudit storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressLoadingAuditDependencyException =
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: failedAddressLoadingAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingAuditTask =
                this.addressLoadingAuditService.AddAddressLoadingAuditAsync(someAddressLoadingAudit);

            AddressLoadingAuditDependencyException actualAddressLoadingAuditDependencyException =
                await Assert.ThrowsAsync<AddressLoadingAuditDependencyException>(
                    addAddressLoadingAuditTask.AsTask);

            // then
            actualAddressLoadingAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}