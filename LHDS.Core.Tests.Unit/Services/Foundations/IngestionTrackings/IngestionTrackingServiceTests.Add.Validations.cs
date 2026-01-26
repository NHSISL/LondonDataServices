// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingIsNullAndLogItAsync()
        {
            // given
            IngestionTracking nullIngestionTracking = null;

            var nullIngestionTrackingException =
                new NullIngestionTrackingException(message: "Ingestion tracking is null.");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: nullIngestionTrackingException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(nullIngestionTracking))
                    .ReturnsAsync(nullIngestionTracking);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(nullIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(nullIngestionTracking),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

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
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingIsInvalidAndLogItAsync(string invalidText)
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
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

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
                values:
                [
                    "Date is required",
                    "Date is not recent"
                ]);

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' " +
                    $"but found '{invalidIngestionTracking.CreatedBy}'."
                ]);

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: "Date is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                values: "Text is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            invalidIngestionTracking.UpdatedDate =
                invalidIngestionTracking.CreatedDate.AddDays(randomNumber);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is not the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIdsIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(randomDateTimeOffset, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;
            invalidIngestionTracking.UpdatedBy = Guid.NewGuid().ToString();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                values: $"Text is not the same as {nameof(IngestionTracking.CreatedBy)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            IngestionTracking randomIngestionTracking =
                CreateRandomIngestionTracking(invalidDateTime, randomUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
        }
    }
}