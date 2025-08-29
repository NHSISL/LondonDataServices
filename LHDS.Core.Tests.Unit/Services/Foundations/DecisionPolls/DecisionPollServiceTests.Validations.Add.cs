// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using LHDS.Core.Services.Foundations.DecisionPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDecisionPollIsNullAndLogItAsync()
        {
            // given
            DecisionPoll nullDecisionPoll = null;

            var nullDecisionPollException =
                new NullDecisionPollException(message: "DecisionPoll is null.");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: nullDecisionPollException);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                this.decisionPollService.AddDecisionPollAsync(nullDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(addDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogErrorAsync(It.Is(SameExceptionAs(
                        expectedDecisionPollValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDecisionPollsIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidDecisionPoll = new DecisionPoll();

            var decisionPollServiceMock = new Mock<DecisionPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            decisionPollServiceMock.Setup(service =>
                service.ApplyAddDecisionPollAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidDecisionPoll.CreatedBy}'."
                ]);

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values: "Date is required");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedBy),
                values: "Text is required");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                decisionPollServiceMock.Object.AddDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(addDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            decisionPollServiceMock.Verify(service =>
                service.ApplyAddDecisionPollAsync(invalidDecisionPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            decisionPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll =
                CreateRandomDecisionPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            DecisionPoll invalidDecisionPoll = randomDecisionPoll;
            invalidDecisionPoll.CreatedDate = GetRandomDateTimeOffset();
            invalidDecisionPoll.UpdatedDate = GetRandomDateTimeOffset();

            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedDate),
                values: $"Date is not the same as {nameof(DecisionPoll.CreatedDate)}");

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.CreatedDate),
                values: "Date is not recent");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            var decisionPollServiceMock = new Mock<DecisionPollService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            decisionPollServiceMock.Setup(service =>
                service.ApplyAddDecisionPollAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<DecisionPoll> addDecisionPollTask =
                decisionPollServiceMock.Object.AddDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(addDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            decisionPollServiceMock.Verify(service =>
                service.ApplyAddDecisionPollAsync(invalidDecisionPoll),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(It.IsAny<DecisionPoll>()),
                    Times.Never);

            decisionPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
