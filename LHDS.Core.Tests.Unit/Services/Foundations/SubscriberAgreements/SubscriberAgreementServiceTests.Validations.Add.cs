// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsNullAndLogItAsync()
        {
            // given
            SubscriberAgreement nullSubscriberAgreement = null;

            var nullSubscriberAgreementException =
                new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: nullSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(nullSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidSubscriberAgreement = new SubscriberAgreement
            {
                SupplierSharingAgreementShortName = invalidText,
                CreatedBy = invalidText,
                UpdatedBy = invalidText
            };

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.Id),
                values: "Id is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                values: "Date is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: "Date is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: "Text is required");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsInvalidLengthsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement invalidSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset);
            invalidSubscriberAgreement.SupplierSharingAgreementShortName = GetRandomString(129);
            invalidSubscriberAgreement.CreatedBy = GetRandomString(256);
            invalidSubscriberAgreement.UpdatedBy = invalidSubscriberAgreement.CreatedBy;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text exceeded length requirement");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: "Text exceeded length requirement");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: "Text exceeded length requirement");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
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
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset);
            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;

            invalidSubscriberAgreement.UpdatedDate =
                invalidSubscriberAgreement.CreatedDate.AddDays(randomNumber);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: $"Date is not the same as {nameof(SubscriberAgreement.CreatedDate)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset);
            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;
            invalidSubscriberAgreement.UpdatedBy = Guid.NewGuid().ToString();

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: $"Text is not the same as {nameof(SubscriberAgreement.CreatedBy)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
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

            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(invalidDateTime);
            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                values: "Date is not recent");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}