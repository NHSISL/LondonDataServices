// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowNullDecisionsExceptionOnGetPatientDecisionsIfDecisionsIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            List<Decision> nullDecisions = null;

            var nullDecisionException =
                new NullDecisionsException(message: "Decisions is null.");

            var expectedDecisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: nullDecisionException);

            this.decisionBrokerMock.Setup(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset))
                    .ReturnsAsync(nullDecisions);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionService.GetPatientDecisions(inputDateTimeOffset);

            DecisionValidationException actualDecisionValidationException =
                await Assert.ThrowsAsync<DecisionValidationException>(() =>
                    getPatientDecisionsTask.AsTask());

            // then
            actualDecisionValidationException.Should()
                .BeEquivalentTo(expectedDecisionValidationException);

            this.decisionBrokerMock.Verify(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.decisionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnGetPatientDecisionsIfDecisionsIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            var invalidDecisions = new List<Decision>
            {
                new()
                {
                    DecisionType = null,
                    Patient = null
                }
            };

            var invalidDecisionException =
                new InvalidDecisionsException(
                    message: "Invalid decisions. Please correct the errors and try again.");

            invalidDecisionException.AddData(
                key: nameof(Decision.DecisionType),
                values: "DecisionType is required");

            invalidDecisionException.AddData(
                key: nameof(Decision.Patient),
                values: "Patient is required");

            var expectedDecisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: invalidDecisionException);

            this.decisionBrokerMock.Setup(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset))
                    .ReturnsAsync(invalidDecisions);

            // when
            ValueTask<List<Decision>> addDecisionTask =
                this.decisionService.GetPatientDecisions(inputDateTimeOffset);

            DecisionValidationException actualDecisionValidationException =
                await Assert.ThrowsAsync<DecisionValidationException>(() =>
                    addDecisionTask.AsTask());

            // then
            actualDecisionValidationException.Should()
                .BeEquivalentTo(expectedDecisionValidationException);

            this.decisionBrokerMock.Verify(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.decisionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
