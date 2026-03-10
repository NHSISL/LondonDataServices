// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingIsNullAndLogItAsync()
        {
            // given
            IngestionTracking nullIngestionTracking = null;
            var nullIngestionTrackingException = new NullIngestionTrackingException(
                message: "Ingestion tracking is null.");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: nullIngestionTrackingException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nullIngestionTracking))
                    .ReturnsAsync(nullIngestionTracking);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(nullIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullIngestionTracking),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidIngestionTracking = new IngestionTracking
            {
                FileName = invalidText,
                EncryptedFileName = invalidText,
                DecryptedFileName = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Id),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.FileName),
                values: "Text is required");


            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.SupplierId),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.EncryptedFileName),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.DecryptedFileName),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: "Date is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        "Date is not recent"
                    ]);

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                 values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomUserId}' but found " +
                            $"'{invalidIngestionTracking.UpdatedBy}'."
                    ]);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            //then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTimeOffset, randomUserId);

            randomIngestionTracking.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(randomIngestionTracking))
                    .ReturnsAsync(randomIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingValidatonException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(randomIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking nonExistIngestionTracking = randomIngestionTracking;
            IngestionTracking nullIngestionTracking = null;

            var notFoundIngestionTrackingException =
                new NotFoundIngestionTrackingException(nonExistIngestionTracking.Id);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistIngestionTracking))
                    .ReturnsAsync(nonExistIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: notFoundIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(nonExistIngestionTracking.Id))
                .ReturnsAsync(nullIngestionTracking);

            // when 
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(nonExistIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(nonExistIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking storageIngestionTracking = invalidIngestionTracking.DeepClone();
            storageIngestionTracking.CreatedDate = storageIngestionTracking.CreatedDate.AddMinutes(randomMinutes);
            storageIngestionTracking.UpdatedDate = storageIngestionTracking.UpdatedDate.AddMinutes(randomMinutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: $"Date is not the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking, storageIngestionTracking))
                        .ReturnsAsync(invalidIngestionTracking);
            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking, storageIngestionTracking),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking storageIngestionTracking = invalidIngestionTracking.DeepClone();
            invalidIngestionTracking.CreatedBy = Guid.NewGuid().ToString();
            storageIngestionTracking.UpdatedDate = storageIngestionTracking.CreatedDate;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values: $"Text is not the same as {nameof(IngestionTracking.CreatedBy)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking, storageIngestionTracking))
                        .ReturnsAsync(invalidIngestionTracking);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should().
                BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking, storageIngestionTracking),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTracking.UpdatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking,
                    storageIngestionTracking))
                        .ReturnsAsync(invalidIngestionTracking);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingService.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            // then
            await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                modifyIngestionTrackingTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTracking,
                    storageIngestionTracking),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}