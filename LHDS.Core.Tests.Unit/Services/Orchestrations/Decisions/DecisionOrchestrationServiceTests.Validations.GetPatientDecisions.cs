// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDecryptIfBlobContainerIsNullAndLogItAsync()
        {
            // given
            var nullBlobContainersDecisionOrchestrationException =
                new NullBlobContainersDecisionOrchestrationException(
                    message: "Null blob container decision orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDecisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersDecisionOrchestrationException);

            BlobContainers invalidBlobContainers = null;

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationValidationException>(getPatientDecisionsTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecisionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationValidationException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
