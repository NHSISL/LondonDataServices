// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            SqlException sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedIngestionTrackingStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(someIngestionTracking);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfIngestionTrackingAlreadyExsitsAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking alreadyExistsIngestionTracking = randomIngestionTracking;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsIngestionTrackingException =
                new AlreadyExistsIngestionTrackingException(
                    message: "Ingestion tracking with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: alreadyExistsIngestionTrackingException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(alreadyExistsIngestionTracking))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(alreadyExistsIngestionTracking);

            // then
            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    addIngestionTrackingTask.AsTask);

            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(alreadyExistsIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidIngestionTrackingReferenceException =
                new InvalidIngestionTrackingReferenceException(
                    message: "Invalid ingestion tracking reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    invalidIngestionTrackingReferenceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(someIngestionTracking);

            // then
            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    addIngestionTrackingTask.AsTask);

            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(someIngestionTracking),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();

            var databaseUpdateException =
                new DbUpdateException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException: failedIngestionTrackingStorageException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(someIngestionTracking);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            var serviceException = new Exception();

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(
                    message: "Failed ingestion tracking service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException: failedIngestionTrackingServiceException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(someIngestionTracking);

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                await Assert.ThrowsAsync<IngestionTrackingServiceException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(someIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingServiceException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}