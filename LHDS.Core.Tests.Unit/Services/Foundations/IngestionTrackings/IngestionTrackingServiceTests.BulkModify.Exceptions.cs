// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnBulkModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedIngestionTrackingStorageException);

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask bulkModifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    bulkModifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnBulkModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidIngestionTrackingReferenceException =
                new InvalidIngestionTrackingReferenceException(
                    message: "Invalid ingestion tracking reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            IngestionTrackingDependencyValidationException expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: invalidIngestionTrackingReferenceException);

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask bulkModifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    bulkModifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnBulkModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            var databaseUpdateException = new DbUpdateException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedIngestionTrackingStorageException);

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask bulkModifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    bulkModifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnBulkModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedIngestionTrackingException =
                new LockedIngestionTrackingException(
                    message: "Locked ingestion tracking record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: lockedIngestionTrackingException);

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask bulkModifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    bulkModifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnBulkModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            var serviceException = new Exception();

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(
                    message: "Failed ingestion tracking service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException: failedIngestionTrackingServiceException);

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask bulkModifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                await Assert.ThrowsAsync<IngestionTrackingServiceException>(
                    bulkModifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(It.IsAny<List<IngestionTracking>>(), It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}