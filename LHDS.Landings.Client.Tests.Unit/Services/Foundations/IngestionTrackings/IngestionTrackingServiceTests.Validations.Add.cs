// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions;
using Moq;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingIsNullAndLogItAsync()
        {
            // given
            IngestionTracking nullIngestionTracking = null;

            var nullIngestionTrackingException =
                new NullIngestionTrackingException();

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(nullIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(nullIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(() =>
                    addIngestionTrackingTask.AsTask());

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidIngestionTracking = new IngestionTracking
            {
                Name = invalidText,
            };

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Id),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Name),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.EncryptedBlobId),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.DecryptedBlobId),
                values: "Needs to be GUID");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: "Date is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: "Date is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                values: "Text is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(() =>
                    addIngestionTrackingTask.AsTask());

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            invalidIngestionTracking.UpdatedDate =
                invalidIngestionTracking.CreatedDate.AddDays(randomNumber);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is not the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(() =>
                    addIngestionTrackingTask.AsTask());

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);


            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfMaxLengthExceededAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            IngestionTracking invalidIngestionTracking = randomIngestionTracking;
            invalidIngestionTracking.Name = GetRandomMessage(1, 256, 500);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException();

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Name),
                values: "Text is exceeding max length");

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(() =>
                    addIngestionTrackingTask.AsTask());

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedAndUpdatedByIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            IngestionTracking invalidIngestionTracking = randomIngestionTracking;
            invalidIngestionTracking.UpdatedBy = GetRandomMessage();

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                values: $"User is not the same as {nameof(IngestionTracking.CreatedBy)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
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

            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(invalidDateTime);
            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(() =>
                    addIngestionTrackingTask.AsTask());

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDecryptedBlobIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            randomIngestionTracking.DecryptedBlobId = Guid.Empty;
            IngestionTracking inputIngestionTracking = randomIngestionTracking;

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException();

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.DecryptedBlobId),
                values: "Id is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> addIngestionTrackingTask =
                this.ingestionTrackingService.AddIngestionTrackingAsync(inputIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    addIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
