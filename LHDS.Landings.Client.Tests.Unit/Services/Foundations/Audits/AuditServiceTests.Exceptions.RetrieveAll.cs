using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedAuditStorageException(sqlException);

            var expectedAuditDependencyException =
                new AuditDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAudits())
                    .Throws(sqlException);

            // when
            Action retrieveAllAuditsAction = () =>
                this.auditService.RetrieveAllAudits();

            AuditDependencyException actualAuditDependencyException =
                Assert.Throws<AuditDependencyException>(retrieveAllAuditsAction);

            // then
            actualAuditDependencyException.Should()
                .BeEquivalentTo(expectedAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAudits(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}