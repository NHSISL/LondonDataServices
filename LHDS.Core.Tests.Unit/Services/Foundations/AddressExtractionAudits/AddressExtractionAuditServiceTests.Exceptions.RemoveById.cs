using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
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
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<AddressExtractionAudit> addAddressExtractionAuditTask =
                this.addressExtractionAuditService.RemoveAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id);

            AddressExtractionAuditDependencyException actualAddressExtractionAuditDependencyException =
                await Assert.ThrowsAsync<AddressExtractionAuditDependencyException>(
                    addAddressExtractionAuditTask.AsTask);

            // then
            actualAddressExtractionAuditDependencyException.Should()
                .BeEquivalentTo(expectedAddressExtractionAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(randomAddressExtractionAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressExtractionAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}