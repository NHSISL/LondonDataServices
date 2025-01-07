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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidOptOut = new OptOut
            {
                NhsNumber = invalidText,
                Status = invalidText,
            };

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values:
                    new[] {
                        "Text is required",
                        "NHS Number invalid"
                    });

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
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(OptOut.CreatedDate)}"
                });

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is required");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutLengthValidationIsInvalidAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            int nhsNumberMaxLength = 10;
            int optOutStatusMaxLength = 50;
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            storageOptOut.UpdatedDate = randomOptOut.CreatedDate;
            inputOptOut.NhsNumber = GetRandomString(length: nhsNumberMaxLength + 1);
            inputOptOut.Status = GetRandomString(length: optOutStatusMaxLength + 1);
            inputOptOut.CreatedBy = GetRandomString(256);
            inputOptOut.UpdatedBy = inputOptOut.CreatedBy;
            OptOut updatedOptOut = inputOptOut;
            OptOut expectedOptOut = updatedOptOut.DeepClone();
            Guid optOutId = inputOptOut.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values:
                    new[] {
                        $"Text length should not be greater than {nhsNumberMaxLength}",
                        "NHS Number invalid"
                    });

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: $"Text length should not be greater than {optOutStatusMaxLength}");

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

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(inputOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfNhsNumberIsInvalidAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut inputOptOut = randomOptOut;
            OptOut storageOptOut = inputOptOut.DeepClone();
            storageOptOut.UpdatedDate = randomOptOut.CreatedDate;
            inputOptOut.NhsNumber = GenerateInvalidNhsNumber();
            OptOut updatedOptOut = inputOptOut;
            OptOut expectedOptOut = updatedOptOut.DeepClone();
            Guid optOutId = inputOptOut.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: $"NHS Number invalid");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(inputOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut invalidOptOut = randomOptOut;

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            randomOptOut.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidOptOutException =
                new InvalidOptOutException(message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is not recent");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(randomOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut nonExistOptOut = randomOptOut;
            OptOut nullOptOut = null;

            var notFoundOptOutException =
                new NotFoundOptOutException(message: $"Couldn't find optOut with optOutId: {nonExistOptOut.Id}.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: notFoundOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(nonExistOptOut.Id))
                .ReturnsAsync(nullOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(nonExistOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(nonExistOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

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
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            storageOptOut.CreatedDate = storageOptOut.CreatedDate.AddMinutes(randomMinutes);
            storageOptOut.UpdatedDate = storageOptOut.UpdatedDate.AddMinutes(randomMinutes);

            var invalidOptOutException = new InvalidOptOutException(
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            invalidOptOut.CreatedBy = Guid.NewGuid().ToString();
            storageOptOut.UpdatedDate = storageOptOut.CreatedDate;

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: $"Text is not the same as {nameof(OptOut.CreatedBy)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                .ReturnsAsync(storageOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should().BeEquivalentTo(expectedOptOutValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomModifyOptOut(randomDateTimeOffset);
            OptOut invalidOptOut = randomOptOut;
            OptOut storageOptOut = randomOptOut.DeepClone();

            var invalidOptOutException = new InvalidOptOutException(
                message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.UpdatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                .ReturnsAsync(storageOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            // then
            await Assert.ThrowsAsync<OptOutValidationException>(
                modifyOptOutTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}