// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;
            var nullOptOutException = new NullOptOutException(message: "OptOut is null.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: nullOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullOptOut),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidOptOut = new OptOut
            {
                Status = invalidText,
                NhsNumber = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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
                key: nameof(OptOut.NhsNumber),
                values:
                [
                    "Text is required",
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: "Date is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomUserId}' but found " +
                        $"'{invalidOptOut.UpdatedBy}'."
                    ]);

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            OptOut invalidOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomUserId);

            var inputCreatedByUpdatedByString = randomUserId;
            invalidOptOut.NhsNumber = GetRandomString(11);
            invalidOptOut.Status = GetRandomString(51);
            invalidOptOut.CreatedBy = inputCreatedByUpdatedByString;
            invalidOptOut.UpdatedBy = inputCreatedByUpdatedByString;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

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
                values: $"Text length should not be greater than 50");

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
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

            OptOut invalidOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomUserId);

            invalidOptOut.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is not recent");

            var expectedOptOutValidatonException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut invalidOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomUserId);

            OptOut nonExistOptOut = invalidOptOut;

            var notFoundOptOutException = new NotFoundOptOutException(
                message: $"Couldn't find optOut with optOutId: {nonExistOptOut.Id}.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: notFoundOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when 
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(nonExistOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistOptOut),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(nonExistOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            storageOptOut.CreatedDate = storageOptOut.CreatedDate.AddMinutes(randomMinutes);
            storageOptOut.UpdatedDate = storageOptOut.UpdatedDate.AddMinutes(randomMinutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: $"Date is not the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            invalidOptOut.CreatedBy = Guid.NewGuid().ToString();
            storageOptOut.UpdatedDate = storageOptOut.CreatedDate;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: $"Text is not the same as {nameof(OptOut.CreatedBy)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should().BeEquivalentTo(expectedOptOutValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
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

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomUserId);

            OptOut invalidOptOut = randomOptOut;
            OptOut storageOptOut = randomOptOut.DeepClone();
            invalidOptOut.UpdatedDate = storageOptOut.UpdatedDate;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.UpdatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutService.ModifyOptOutAsync(invalidOptOut);

            // then
            await Assert.ThrowsAsync<OptOutValidationException>(
                modifyOptOutTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidOptOut),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}