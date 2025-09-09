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
        public async Task ShouldThrowServiceExceptionOnGetPatientDecisionsIfServiceErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            var serviceException = new Exception();

            var failedDecisionServiceException =
                new FailedDecisionServiceException(
                    message: "Failed decision service occurred, please contact support",
                    innerException: serviceException);

            var expectedDecisionServiceException =
                new DecisionServiceException(
                    message: "Decision service error occurred, contact support.",
                    innerException: failedDecisionServiceException);

            this.decisionBrokerMock.Setup(broker =>
                broker.GetPatientDecisions(It.IsAny<DateTimeOffset>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionService.GetPatientDecisions(inputDateTimeOffset);

            DecisionServiceException actualDecisionServiceException =
                await Assert.ThrowsAsync<DecisionServiceException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualDecisionServiceException.Should()
                .BeEquivalentTo(expectedDecisionServiceException);

            this.decisionBrokerMock.Verify(broker =>
                broker.GetPatientDecisions(It.IsAny<DateTimeOffset>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.decisionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
