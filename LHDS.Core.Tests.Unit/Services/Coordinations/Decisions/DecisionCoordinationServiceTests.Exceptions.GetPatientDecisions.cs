// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decisions
{
    public partial class DecisionCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(DecisionCoordinationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnGetPatientDecisionsIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDependencyException =
                new DecisionCoordinationDependencyValidationException(
                    message: "Decision coordination dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.decisionOrchestrationServiceMock.Setup(service =>
                service.GetPatientDecisions())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionCoordinationService.GetPatientDecisions();

            DecisionCoordinationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecisionCoordinationDependencyValidationException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.decisionOrchestrationServiceMock.Verify(service =>
                service.GetPatientDecisions(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DecisionCoordinationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGetPatientDecisionsIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDependencyException =
                new DecisionCoordinationDependencyException(
                    message:
                    "Decision coordination dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.decisionOrchestrationServiceMock.Setup(service =>
                service.GetPatientDecisions())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionCoordinationService.GetPatientDecisions();

            DecisionCoordinationDependencyException actualException =
                await Assert.ThrowsAsync<DecisionCoordinationDependencyException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.decisionOrchestrationServiceMock.Verify(service =>
                service.GetPatientDecisions(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
