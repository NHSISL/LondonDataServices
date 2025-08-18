// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberPracticeIsNullAndLogItAsync()
        {
            // given
            SubscriberPractice nullSubscriberPractice = null;

            var nullSubscriberPracticeException =
                new NullSubscriberPracticeException(message: "SubscriberPractice is null.");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: nullSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                this.subscriberPracticeService.AddSubscriberPracticeAsync(nullSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberPracticeIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidSubscriberPractice = new SubscriberPractice
            {
                Name = invalidText,
                PracticeCode = invalidText,
            };

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.Id),
                values: "Id is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.SubscriberAgreementId),
                values: "Id is required");

            invalidSubscriberPracticeException.AddData(
               key: nameof(SubscriberPractice.Name),
               values: "Text is required");

            invalidSubscriberPracticeException.AddData(
               key: nameof(SubscriberPractice.PracticeCode),
               values: "Text is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedDate),
                 values:
                [
                    "Date is required",
                    "Date is not recent"
                ]);

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidSubscriberPractice.CreatedBy}'."
                ]);

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values: "Date is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedBy),
                values: "Text is required");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.AddSubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);
            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice;

            invalidSubscriberPractice.UpdatedDate =
                invalidSubscriberPractice.CreatedDate.AddDays(randomNumber);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values: $"Date is not the same as {nameof(SubscriberPractice.CreatedDate)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.AddSubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);
            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice;
            invalidSubscriberPractice.UpdatedBy = Guid.NewGuid().ToString();

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedBy),
                values: $"Text is not the same as {nameof(SubscriberPractice.CreatedBy)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.AddSubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);
            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice;

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedDate),
                values: "Date is not recent");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(invalidDateTime);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<SubscriberPractice> addSubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.AddSubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(addSubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}