// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(nullSubscriberAgreement),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidSubscriberAgreement = new SubscriberAgreement
            {
                SupplierSharingAgreementShortName = invalidText,
            };

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.Id),
                values: "Id is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierId),
                values: "Id is required");

            invalidSubscriberAgreementException.AddData(
               key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
               values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                 values:
                [
                    "Date is required",
                    "Date is not recent"
                ]);

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found '{invalidSubscriberAgreement.CreatedBy}'."
                ]);

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
                subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsInvalidLengthAndLogItAsync()
        {
            // given 
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement invalidSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);
            invalidSubscriberAgreement.SupplierSharingAgreementShortName = GetRandomString(129);
            invalidSubscriberAgreement.CreatedBy = GetRandomString(256);
            invalidSubscriberAgreement.UpdatedBy = invalidSubscriberAgreement.CreatedBy;

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text exceeded length requirement");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                [
                    "Text exceeded length requirement",
                    $"Expected value to be '{randomUserId}' but found '{invalidSubscriberAgreement.CreatedBy}'."
                ]);

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: "Text exceeded length requirement");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    addSubscriberAgreementTask.AsTask);

            //then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);
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

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);
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

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);
            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(invalidDateTime);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}