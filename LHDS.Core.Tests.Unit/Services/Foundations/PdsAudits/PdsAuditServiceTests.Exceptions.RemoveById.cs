using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            SqlException sqlException = GetSqlException();

            var failedPdsAuditStorageException =
                new FailedPdsAuditStorageException(sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(failedPdsAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id))
                    .Throws(sqlException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(randomPdsAudit.Id);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(randomPdsAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(It.IsAny<PdsAudit>()),
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