// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}
