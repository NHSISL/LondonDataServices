// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditIsNullAndLogItAsync()
        {
            // given
            IngestionTrackingAudit nullIngestionTrackingAudit = null;

            var nullIngestionTrackingAuditException =
                new NullIngestionTrackingAuditException(message: "IngestionTrackingAudit is null.");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: nullIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidIngestionTrackingAudit = new IngestionTrackingAudit
            {
                Message = invalidText
            };

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Id),
                values: "Id is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.IngestionTrackingId),
                values: "Id is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Message),
                values: "Text is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: "Date is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: "Text is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: "Date is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: "Text is required");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;

            invalidIngestionTrackingAudit.UpdatedDate =
                invalidIngestionTrackingAudit.CreatedDate.AddDays(randomNumber);

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIdsIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            invalidIngestionTrackingAudit.UpdatedBy = Guid.NewGuid().ToString();

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: $"Text is not the same as {nameof(IngestionTrackingAudit.CreatedBy)}");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit(invalidDateTime);
            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: "Date is not recent");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedByAndUpdatedByIsInvalidLengthAndLogItAsync()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            invalidIngestionTrackingAudit.CreatedBy = GetRandomString(256);
            invalidIngestionTrackingAudit.UpdatedBy = invalidIngestionTrackingAudit.CreatedBy;

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: "Text is exceeding max length");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> addAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(addAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}