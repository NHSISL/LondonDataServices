// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            var sqlException = GetSqlException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(sqlException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedIngestionTrackingStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .Throws(sqlException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(randomIngestionTracking),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking someIngestionTracking = CreateRandomIngestionTracking();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidIngestionTrackingReferenceException =
                new InvalidIngestionTrackingReferenceException(foreignKeyConstraintConflictException);

            IngestionTrackingDependencyValidationException expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(invalidIngestionTrackingReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(someIngestionTracking.Id))
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(someIngestionTracking);

            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(someIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedIngestionTrackingDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(someIngestionTracking),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            var databaseUpdateException = new DbUpdateException();

            var failedIngestionTrackingStorageException =
                new FailedIngestionTrackingStorageException(databaseUpdateException);

            var expectedIngestionTrackingDependencyException =
                new IngestionTrackingDependencyException(failedIngestionTrackingStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingDependencyException actualIngestionTrackingDependencyException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(randomIngestionTracking),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedIngestionTrackingException =
                new LockedIngestionTrackingException(databaseUpdateConcurrencyException);

            var expectedIngestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(lockedIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingDependencyValidationException actualIngestionTrackingDependencyValidationException =
                await Assert.ThrowsAsync<IngestionTrackingDependencyValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingDependencyValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(randomIngestionTracking),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            var serviceException = new Exception();

            var failedIngestionTrackingServiceException =
                new FailedIngestionTrackingServiceException(serviceException);

            var expectedIngestionTrackingServiceException =
                new IngestionTrackingServiceException(failedIngestionTrackingServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .Throws(serviceException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingServiceException actualIngestionTrackingServiceException =
                await Assert.ThrowsAsync<IngestionTrackingServiceException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingServiceException.Should()
                .BeEquivalentTo(expectedIngestionTrackingServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(randomIngestionTracking),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}