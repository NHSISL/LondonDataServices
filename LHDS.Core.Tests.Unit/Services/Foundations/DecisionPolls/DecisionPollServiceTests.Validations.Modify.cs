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
        public async Task ShouldThrowValidationExceptionOnModifyIfDecisionPollIsNullAndLogItAsync()
        {
            // given
            DecisionPoll nullDecisionPoll = null;
            var nullDecisionPollException = new NullDecisionPollException(message: "DecisionPoll is null.");

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: nullDecisionPollException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                this.decisionPollService.ModifyDecisionPollAsync(nullDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            // then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDecisionPollIsInvalidAndLogItAsync()
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
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
                service.ApplyModifyDecisionPollAsync(invalidDecisionPoll))
                    .ReturnsAsync(invalidDecisionPoll);

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
                        "Date is the same as CreatedDate",
                        "Date is not recent"
                    ]);

            invalidDecisionPollException.AddData(
                key: nameof(DecisionPoll.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidDecisionPoll.UpdatedBy}'."
                    ]);

            var expectedDecisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: invalidDecisionPollException);

            // when
            ValueTask<DecisionPoll> modifyDecisionPollTask =
                decisionPollServiceMock.Object.ModifyDecisionPollAsync(invalidDecisionPoll);

            DecisionPollValidationException actualDecisionPollValidationException =
                await Assert.ThrowsAsync<DecisionPollValidationException>(
                    modifyDecisionPollTask.AsTask);

            //then
            actualDecisionPollValidationException.Should()
                .BeEquivalentTo(expectedDecisionPollValidationException);

            decisionPollServiceMock.Verify(service =>
                service.ApplyModifyDecisionPollAsync(invalidDecisionPoll),
                    Times.Once());

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

            decisionPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
