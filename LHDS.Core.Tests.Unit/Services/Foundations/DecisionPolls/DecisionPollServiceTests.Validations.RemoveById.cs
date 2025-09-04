// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
