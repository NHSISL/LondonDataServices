// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressAuditStorageException =
                new FailedResolvedAddressAuditStorageException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressAuditDependencyException =
                new ResolvedAddressAuditDependencyException(
                    message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ResolvedAddressAudit> retrieveResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(someId);

            ResolvedAddressAuditDependencyException actualResolvedAddressAuditDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressAuditDependencyException>(
                    retrieveResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedResolvedAddressAuditServiceException =
                new FailedResolvedAddressAuditServiceException(
                    message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressAuditServiceException =
                new ResolvedAddressAuditServiceException(
                    message: "ResolvedAddressAudit service error occurred, please contact support.",
                    innerException: failedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ResolvedAddressAudit> retrieveResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(someId);

            ResolvedAddressAuditServiceException actualResolvedAddressAuditServiceException =
                await Assert.ThrowsAsync<ResolvedAddressAuditServiceException>(
                    retrieveResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditServiceException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressAuditServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}