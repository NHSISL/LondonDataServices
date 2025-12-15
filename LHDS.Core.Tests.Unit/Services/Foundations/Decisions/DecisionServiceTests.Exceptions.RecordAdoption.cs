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
        public async Task ShouldThrowServiceExceptionOnRecordAdoptionsIfServiceErrorOccursAndLogItAsync()
        {
            // given
            List<Decision> randomDecisionsAdopted = CreateRandomDecisions();
            List<Decision> inputDecisionsAdopted = randomDecisionsAdopted;
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
                broker.RecordAdoption(It.IsAny<List<Decision>>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask recordAdoptionTask = this.decisionService.RecordAdoption(inputDecisionsAdopted);

            DecisionServiceException actualDecisionServiceException =
                await Assert.ThrowsAsync<DecisionServiceException>(
                    recordAdoptionTask.AsTask);

            // then
            actualDecisionServiceException.Should()
                .BeEquivalentTo(expectedDecisionServiceException);

            this.decisionBrokerMock.Verify(broker =>
                broker.RecordAdoption(It.IsAny<List<Decision>>()),
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
