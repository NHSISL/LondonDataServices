// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedIngestionTrackingAuditStorageException(
                    message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingAuditDependencyException =
                new IngestionTrackingAuditDependencyException(
                    message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync())
                    .Throws(sqlException);

            // when
            ValueTask<IQueryable<IngestionTrackingAudit>> retrieveAllIngestionTrackingAuditsTask = 
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync();

            IngestionTrackingAuditDependencyException actualIngestionTrackingAuditDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingAuditDependencyException>(
                    retrieveAllIngestionTrackingAuditsTask.AsTask);

            // then
            actualIngestionTrackingAuditDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedIngestionTrackingAuditServiceException =
                new FailedIngestionTrackingAuditServiceException(
                    message: "Failed IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingAuditServiceException =
                new IngestionTrackingAuditServiceException(
                    message: "IngestionTrackingAudit service error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync())
                    .Throws(serviceException);

            // when
            ValueTask<IQueryable<IngestionTrackingAudit>> retrieveAllIngestionTrackingAuditsTask =
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync();

            IngestionTrackingAuditServiceException actualIngestionTrackingAuditServiceException =
                await Assert.ThrowsAsync<IngestionTrackingAuditServiceException>(
                    retrieveAllIngestionTrackingAuditsTask.AsTask);

            // then
            actualIngestionTrackingAuditServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}