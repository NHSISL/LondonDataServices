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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidDecisionPollId = Guid.Empty;

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.Id),
                values: "Id is required");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            // when
            ValueTask<DecisionPoll> removeDecisionPollByIdTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(invalidDecisionPollId);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    removeDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        expectedDecisionPollValidationException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfDecisionPollDoesNotExistAndLogItAsync()
        {
            // given
            Guid someDecisionPollId = Guid.NewGuid();
            DecisionPoll nullDecisionPoll = null;
            var notFoundDecisionPollException = new NotFoundDecisionPollException(someDecisionPollId);

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: notFoundDecisionPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(someDecisionPollId))
                    .ReturnsAsync(nullDecisionPoll);

            // when
            ValueTask<DecisionPoll> removeDecisionPollByIdTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(someDecisionPollId);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    removeDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(someDecisionPollId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnRemoveIfUpdatedDecisionPollCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll(
                randomDateTimeOffset,
                randomEntraUser.EntraUserId);

            DecisionPoll storageDecisionPoll = randomDecisionPoll.DeepClone();
            DecisionPoll updatedDecisionPoll = randomDecisionPoll.DeepClone();
            updatedDecisionPoll.CreatedDate = storageDecisionPoll.CreatedDate.AddMinutes(5);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            // when
            ValueTask<DecisionPoll> removeDecisionPollByIdTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(updatedDecisionPoll.Id);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    removeDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnRemoveIfUpdatedDecisionPollCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll(
                randomDateTimeOffset,
                randomEntraUser.EntraUserId);

            DecisionPoll storageDecisionPoll = randomDecisionPoll.DeepClone();
            DecisionPoll updatedDecisionPoll = randomDecisionPoll.DeepClone();
            updatedDecisionPoll.CreatedBy = Guid.NewGuid().ToString();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            // when
            ValueTask<DecisionPoll> removeDecisionPollByIdTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(updatedDecisionPoll.Id);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    removeDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnRemoveIfUpdatedDecisionPollUpdatedDateNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll(
                randomDateTimeOffset,
                randomEntraUser.EntraUserId);

            DecisionPoll storageDecisionPoll = randomDecisionPoll.DeepClone();
            DecisionPoll updatedDecisionPoll = randomDecisionPoll.DeepClone();
            updatedDecisionPoll.UpdatedDate = storageDecisionPoll.UpdatedDate.AddMinutes(10);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values: $"Date is not the same as {nameof(DecisionPoll.UpdatedDate)}");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            // when
            ValueTask<DecisionPoll> removeDecisionPollByIdTask =
                this.decisionPollService.RemoveDecisionPollByIdAsync(updatedDecisionPoll.Id);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    removeDecisionPollByIdTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(updatedDecisionPoll.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(storageDecisionPoll),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}