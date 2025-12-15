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
        public async Task ShouldThrowValidationExceptionOnGetPatientDecisionsIfDecisionsIsInvalidAndLogItAsync()
        {
            // given
            Guid someDecisionId = Guid.NewGuid();

            var invalidDecisions = new List<Decision>
            {
                new()
                {
                    Id = someDecisionId,
                    DecisionTypeName = null,
                    PatientNhsNumber = null
                }
            };

            var invalidDecisionException =
                new InvalidDecisionsException(
                    message: "Invalid decisions. Please correct the errors and try again.");

            invalidDecisionException.AddData(
                key: $"Decisions[0].{nameof(Decision.DecisionTypeName)} - Id: {someDecisionId}",
                values: "Text is required");

            invalidDecisionException.AddData(
                key: $"Decisions[0].{nameof(Decision.PatientNhsNumber)} - Id: {someDecisionId}",
                values: "Text is required");

            var expectedDecisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: invalidDecisionException);

            this.decisionBrokerMock.Setup(broker =>
                broker.GetPatientDecisions())
                    .ReturnsAsync(invalidDecisions);

            // when
            ValueTask<List<Decision>> addDecisionTask =
                this.decisionService.GetPatientDecisions();

            DecisionValidationException actualDecisionValidationException =
                await Assert.ThrowsAsync<DecisionValidationException>(() =>
                    addDecisionTask.AsTask());

            // then
            actualDecisionValidationException.Should()
                .BeEquivalentTo(expectedDecisionValidationException);

            this.decisionBrokerMock.Verify(broker =>
                broker.GetPatientDecisions(),
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
