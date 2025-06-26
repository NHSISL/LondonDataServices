// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedResolvedAddressAuditStorageException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressAuditDependencyException =
                new ResolvedAddressAuditDependencyException(
                    message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressAuditsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<ResolvedAddressAudit>> retrieveAllResolvedAddressAuditsTask =
                this.resolvedAddressAuditService.RetrieveAllResolvedAddressAuditsAsync();

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    testCode: retrieveAllResolvedAddressAuditsTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddressAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditDependencyException))),
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

            var failedResolvedAddressAuditServiceException =
                new FailedResolvedAddressAuditServiceException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressAuditServiceException =
                new ResolvedAddressAuditServiceException(
                    message: "ResolvedAddressAudit service error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressAuditsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<ResolvedAddressAudit>> retrieveAllResolvedAddressAuditsTask =
                this.resolvedAddressAuditService.RetrieveAllResolvedAddressAuditsAsync();

            ResolvedAddressAuditServiceException actualResolvedAddressAuditServiceException =
                await Assert.ThrowsAsync<ResolvedAddressAuditServiceException>(
                    testCode: retrieveAllResolvedAddressAuditsTask.AsTask);

            // then
            actualResolvedAddressAuditServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddressAuditsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}