// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDecisionPollIsNullAndLogItAsync()
        {
            // given
            DecisionPoll nullDecisionPoll = null;
            var nullDecisionPollException = new NullDecisionPollException(message: "DecisionPoll is null.");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: nullDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nullDecisionPoll))
                    .ReturnsAsync(nullDecisionPoll);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(nullDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDecisionPollIsInvalidAndLogItAsync()
        {
            // given 
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            var invalidDecisionPoll = new DecisionPoll();

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.Id),
                values: "Id is required");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.LastPoll),
                values: "Date is required");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedDate),
                values: "Date is required");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedBy),
                values: "Text is required");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values:
                [
                    "Date is required",
                    $"Date is the same as {nameof(DecisionPoll.CreatedDate)}"
                ]);

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUserId}' but found '{invalidDecisionPoll.UpdatedBy}'."
                ]);

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            //then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll =
                CreateRandomDecisionPoll(dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            DecisionPoll invalidDecisionPoll = randomDecisionPoll;

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values: $"Date is the same as {nameof(DecisionPoll.CreatedDate)}");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
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
            DateTimeOffset invalidDate = randomDateTimeOffset.AddMinutes(minutes);
            DateTimeOffset startDate = randomDateTimeOffset.AddSeconds(-90);
            DateTimeOffset endDate = randomDateTimeOffset.AddSeconds(0);
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll =
                CreateRandomDecisionPoll(dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            randomDecisionPoll.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values:
                    $"Date is not recent. Expected a value between {startDate} and {endDate} but found {invalidDate}");

            var expectedDecisionPollValidatonException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(randomDecisionPoll))
                    .ReturnsAsync(randomDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(randomDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(randomDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDecisionPollDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll = CreateRandomModifyDecisionPoll(
                dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            DecisionPoll nonExistDecisionPoll = randomDecisionPoll;
            DecisionPoll nullDecisionPoll = null;

            var notFoundDecisionPollException = new NotFoundDecisionPollException(nonExistDecisionPoll.Id);

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: notFoundDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistDecisionPoll))
                    .ReturnsAsync(nonExistDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(nonExistDecisionPoll.Id))
                    .ReturnsAsync(nullDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(nonExistDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(nonExistDecisionPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll = CreateRandomModifyDecisionPoll(
                dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            DecisionPoll invalidDecisionPoll = randomDecisionPoll.DeepClone();
            DecisionPoll storageDecisionPoll = invalidDecisionPoll.DeepClone();
            storageDecisionPoll.CreatedDate = storageDecisionPoll.CreatedDate.AddMinutes(randomMinutes);
            storageDecisionPoll.UpdatedDate = storageDecisionPoll.UpdatedDate.AddMinutes(randomMinutes);

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedDate),
                values: $"Date is not the same as {nameof(DecisionPoll.CreatedDate)}");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDecisionPollValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedEntraUserDoesntMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll =
                CreateRandomModifyDecisionPoll(dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            DecisionPoll invalidDecisionPoll = randomDecisionPoll.DeepClone();
            DecisionPoll storageDecisionPoll = invalidDecisionPoll.DeepClone();
            invalidDecisionPoll.CreatedBy = Guid.NewGuid().ToString();
            storageDecisionPoll.UpdatedDate = storageDecisionPoll.CreatedDate;

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedBy),
                values: $"Text is not the same as {nameof(DecisionPoll.CreatedBy)}");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should().BeEquivalentTo(expectedDecisionPollValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDecisionPollValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomEntraUserId);

            DecisionPoll randomDecisionPoll =
                CreateRandomModifyDecisionPoll(dateTimeOffset: randomDateTimeOffset, userId: randomEntraUserId);

            DecisionPoll invalidDecisionPoll = randomDecisionPoll;
            DecisionPoll storageDecisionPoll = randomDecisionPoll.DeepClone();

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values: $"Date is the same as {nameof(DecisionPoll.UpdatedDate)}");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(invalidDecisionPoll);

            // then
            await Assert.ThrowsAsync<DecisionPollValidationException>(
                modifyDecisionPollTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(invalidDecisionPoll.Id),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(invalidDecisionPoll, storageDecisionPoll),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
