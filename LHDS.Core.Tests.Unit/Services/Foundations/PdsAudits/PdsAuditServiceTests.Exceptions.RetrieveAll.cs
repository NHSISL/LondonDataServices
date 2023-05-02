using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedPdsAuditStorageException(sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAudits())
                    .Throws(sqlException);

            // when
            Action retrieveAllPdsAuditsAction = () =>
                this.pdsAuditService.RetrieveAllPdsAudits();

            PdsAuditDependencyException actualPdsAuditDependencyException =
                Assert.Throws<PdsAuditDependencyException>(retrieveAllPdsAuditsAction);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAudits(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedPdsAuditServiceException =
                new FailedPdsAuditServiceException(serviceException);

            var expectedPdsAuditServiceException =
                new PdsAuditServiceException(failedPdsAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAudits())
                    .Throws(serviceException);

            // when
            Action retrieveAllPdsAuditsAction = () =>
                this.pdsAuditService.RetrieveAllPdsAudits();

            PdsAuditServiceException actualPdsAuditServiceException =
                Assert.Throws<PdsAuditServiceException>(retrieveAllPdsAuditsAction);

            // then
            actualPdsAuditServiceException.Should()
                .BeEquivalentTo(expectedPdsAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAudits(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}