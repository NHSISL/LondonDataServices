// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decisions
{
    public partial class DecisionServiceTests
    {
        [Fact]
        public async Task ShouldThrowNullDecisionsExceptionOnRecordAdoptionIfDecisionsAdoptedIsNullAndLogItAsync()
        {
            // given

            List<Decision> nullDecisionsAdopted = null;

            var nullDecisionException =
                new NullDecisionsException(message: "DecisionsAdopted is null.");

            var expectedDecisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: nullDecisionException);

            // when
            ValueTask recordAdoptionTask = this.decisionService.RecordAdoption(nullDecisionsAdopted);

            DecisionValidationException actualDecisionValidationException =
                await Assert.ThrowsAsync<DecisionValidationException>(() =>
                    recordAdoptionTask.AsTask());

            // then
            actualDecisionValidationException.Should()
                .BeEquivalentTo(expectedDecisionValidationException);

            this.decisionBrokerMock.Verify(broker =>
                broker.RecordAdoption(nullDecisionsAdopted),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.decisionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowInvalidDecisionsExceptionOnRecordAdoptionIfDecisionsAdoptedIsEmptyAndLogItAsync()
        {
            // given

            List<Decision> emptyDecisionsAdopted = new List<Decision>();

            var nullDecisionException =
                new InvalidDecisionsException(message: "DecisionsAdopted is empty.");

            var expectedDecisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: nullDecisionException);

            // when
            ValueTask recordAdoptionTask = this.decisionService.RecordAdoption(emptyDecisionsAdopted);

            DecisionValidationException actualDecisionValidationException =
                await Assert.ThrowsAsync<DecisionValidationException>(() =>
                    recordAdoptionTask.AsTask());

            // then
            actualDecisionValidationException.Should()
                .BeEquivalentTo(expectedDecisionValidationException);

            this.decisionBrokerMock.Verify(broker =>
                broker.RecordAdoption(emptyDecisionsAdopted),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.decisionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
