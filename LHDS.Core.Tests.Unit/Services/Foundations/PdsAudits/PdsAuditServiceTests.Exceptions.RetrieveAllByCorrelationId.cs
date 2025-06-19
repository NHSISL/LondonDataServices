// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllByCorrelationIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCorrelationId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedPdsAuditStorageException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(
                    message: "PdsAudit dependency error occurred, please contact support.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<PdsAudit>> retrieveAllPdsAuditsTask =
                this.pdsAuditService.RetrieveAllPdsAuditsByCorrelationIdAsync(someCorrelationId);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    testCode: retrieveAllPdsAuditsTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllByCorrelationIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            Guid someCorrelationId = Guid.NewGuid();
            var serviceException = new Exception(exceptionMessage);

            var failedPdsAuditServiceException =
                new FailedPdsAuditServiceException(
                    message: "Failed pdsAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsAuditServiceException =
                new PdsAuditServiceException(
                    message: "PdsAudit service error occurred, please contact support.",
                    innerException: failedPdsAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<PdsAudit>> retrieveAllPdsAuditsTask =
                this.pdsAuditService.RetrieveAllPdsAuditsByCorrelationIdAsync(someCorrelationId);

            PdsAuditServiceException actualPdsAuditServiceException =
                await Assert.ThrowsAsync<PdsAuditServiceException>(
                    testCode: retrieveAllPdsAuditsTask.AsTask);

            // then
            actualPdsAuditServiceException.Should()
                .BeEquivalentTo(expectedPdsAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}