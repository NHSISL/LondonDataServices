using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Audit randomAudit = CreateRandomAudit();
            SqlException sqlException = GetSqlException();

            var failedAuditStorageException =
                new FailedAuditStorageException(sqlException);

            var expectedAuditDependencyException =
                new AuditDependencyException(failedAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAuditByIdAsync(randomAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Audit> addAuditTask =
                this.auditService.RemoveAuditByIdAsync(randomAudit.Id);

            AuditDependencyException actualAuditDependencyException =
                await Assert.ThrowsAsync<AuditDependencyException>(
                    addAuditTask.AsTask);

            // then
            actualAuditDependencyException.Should()
                .BeEquivalentTo(expectedAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAuditByIdAsync(randomAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAuditAsync(It.IsAny<Audit>()),
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