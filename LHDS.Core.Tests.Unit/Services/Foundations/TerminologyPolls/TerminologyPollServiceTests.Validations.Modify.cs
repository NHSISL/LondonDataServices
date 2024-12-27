// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;
            var nullTerminologyPollException = new NullTerminologyPollException(message: "TerminologyPoll is null.");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: nullTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidTerminologyPoll = new TerminologyPoll
            {
                ResourceType = invalidText,
            };

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.ResourceType),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.LastPoll),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(TerminologyPoll.CreatedDate)}"
                });

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            //then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
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
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
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
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            randomTerminologyPoll.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: "Date is not recent");

            var expectedTerminologyPollValidatonException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll nonExistTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll nullTerminologyPoll = null;

            var notFoundTerminologyPollException =
                new NotFoundTerminologyPollException(nonExistTerminologyPoll.Id);

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: notFoundTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(nonExistTerminologyPoll.Id))
                .ReturnsAsync(nullTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(nonExistTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(nonExistTerminologyPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
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
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll.DeepClone();
            TerminologyPoll storageTerminologyPoll = invalidTerminologyPoll.DeepClone();
            storageTerminologyPoll.CreatedDate = storageTerminologyPoll.CreatedDate.AddMinutes(randomMinutes);
            storageTerminologyPoll.UpdatedDate = storageTerminologyPoll.UpdatedDate.AddMinutes(randomMinutes);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: $"Date is not the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                .ReturnsAsync(storageTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTerminologyPollValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll.DeepClone();
            TerminologyPoll storageTerminologyPoll = invalidTerminologyPoll.DeepClone();
            invalidTerminologyPoll.CreatedBy = Guid.NewGuid().ToString();
            storageTerminologyPoll.UpdatedDate = storageTerminologyPoll.CreatedDate;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: $"Text is not the same as {nameof(TerminologyPoll.CreatedBy)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                .ReturnsAsync(storageTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should().BeEquivalentTo(expectedTerminologyPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTerminologyPollValidationException))),
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
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll.DeepClone();

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyPoll.UpdatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id))
                .ReturnsAsync(storageTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            // then
            await Assert.ThrowsAsync<TerminologyPollValidationException>(
                modifyTerminologyPollTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}