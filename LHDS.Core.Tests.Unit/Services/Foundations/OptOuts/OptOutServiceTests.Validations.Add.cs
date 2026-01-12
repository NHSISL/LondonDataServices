// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;

            var nullOptOutException =
                new NullOptOutException(message: "OptOut is null.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: nullOptOutException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(nullOptOut),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            var invalidOptOut = new OptOut
            {
                NhsNumber = invalidText,
                Status = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values:
                [

                    "Text is required",
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                 key: nameof(OptOut.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found '{invalidOptOut.CreatedBy}'."
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is required");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutService.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            OptOut invalidOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomUserId);

            var inputCreatedByUpdatedByString = randomUserId;
            invalidOptOut.NhsNumber = GetRandomString(11);
            invalidOptOut.Status = GetRandomString(51);
            invalidOptOut.CreatedBy = inputCreatedByUpdatedByString;
            invalidOptOut.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values:
                [
                    "Text length should not be greater than 10",
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: "Text length should not be greater than 50");

            invalidOptOutException.AddData(
               key: nameof(OptOut.CreatedBy),
               values: "Text is exceeding max length");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutService.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut;
            invalidOptOut.CreatedDate = GetRandomDateTimeOffset();
            invalidOptOut.UpdatedDate = GetRandomDateTimeOffset();

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is not the same as {nameof(OptOut.CreatedDate)}");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: $"Date is not recent");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutService.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut;
            invalidOptOut.CreatedBy = GetRandomString();
            invalidOptOut.UpdatedBy = GetRandomString();

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: $"Expected value to be '{randomUserId}' " +
                    $"but found '{invalidOptOut.CreatedBy}'.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: $"Text is not the same as {nameof(OptOut.CreatedBy)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutService.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset invalidDateTime = randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            OptOut randomOptOut =
                CreateRandomOptOut(invalidDateTime, randomUserId);

            OptOut invalidOptOut = randomOptOut;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: "Date is not recent");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutService.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidOptOut),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}