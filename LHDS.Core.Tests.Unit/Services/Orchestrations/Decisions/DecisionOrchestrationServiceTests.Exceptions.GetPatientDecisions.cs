// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DecisionDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnGetPatientDecisionsIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDependencyException =
                new DecisionOrchestrationDependencyValidationException(
                    message:
                    "Decision orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.decisionPollServiceMock.Setup(service =>
                service.RetrieveAllDecisionPollsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationDependencyValidationException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.decisionPollServiceMock.Verify(service =>
                service.RetrieveAllDecisionPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DecisionDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGetPatientDecisionsIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDependencyException =
                new DecisionOrchestrationDependencyException(
                    message:
                    "Decision orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.decisionPollServiceMock.Setup(service =>
                service.RetrieveAllDecisionPollsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationDependencyException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.decisionPollServiceMock.Verify(service =>
                service.RetrieveAllDecisionPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetPatientDecisionsIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedDecisionOrchestrationServiceException =
                new FailedDecisionOrchestrationServiceException(
                    message: "Failed decision orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDecisionOrchestrationServiceException =
                new DecisionOrchestrationServiceException(
                    message: "Decision orchestration service error occurred, please contact support.",
                    innerException: failedDecisionOrchestrationServiceException);

            this.decisionPollServiceMock.Setup(service =>
                service.RetrieveAllDecisionPollsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationServiceException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDecisionOrchestrationServiceException);

            this.decisionPollServiceMock.Verify(service =>
                service.RetrieveAllDecisionPollsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationServiceException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}