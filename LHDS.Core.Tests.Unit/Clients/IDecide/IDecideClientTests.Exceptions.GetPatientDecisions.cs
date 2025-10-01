// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Clients.IDecideClient.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Clients.IDecide
{
    public partial class IDecideClientTests
    {
        [Theory]
        [MemberData(nameof(IDecideClientDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnGetPatientDecisionsIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDependencyValidationException = new IDecideClientValidationException(
                message: "IDecide client validation error occurred, fix errors and try again.",
                innerException: dependencyValidationException.InnerException as Xeption);

            this.decisionOrchestrationServiceMock.Setup(orchestration =>
                orchestration.GetPatientDecisions())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.iDecideClient.GetPatientDecisions();

            IDecideClientValidationException actualException =
                await Assert.ThrowsAsync<IDecideClientValidationException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.decisionOrchestrationServiceMock.Verify(orchestration =>
                orchestration.GetPatientDecisions(),
                    Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IDecideClientDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGetPatientDecisionsIfDependencyExceptionOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDependencyException = new IDecideClientDependencyException(
                    message: "IDecide client dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.decisionOrchestrationServiceMock.Setup(orchestration =>
                orchestration.GetPatientDecisions())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.iDecideClient.GetPatientDecisions();

            IDecideClientDependencyException actualException =
                await Assert.ThrowsAsync<IDecideClientDependencyException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.decisionOrchestrationServiceMock.Verify(orchestration =>
                orchestration.GetPatientDecisions(),
                    Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IDecideClientServiceExceptions))]
        public async Task ShouldThrowServiceExceptionOnGetPatientDecisionsIfServiceErrorOccursAndLogItAsync(
            Xeption serviceException)
        {
            // given
            var expectedServiceException = new IDecideClientServiceException(
                message: "IDecide client service error occurred, please contact support.",
                innerException: serviceException.InnerException as Xeption);

            this.decisionOrchestrationServiceMock.Setup(orchestration =>
                orchestration.GetPatientDecisions())
                    .ThrowsAsync(expectedServiceException);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.iDecideClient.GetPatientDecisions();

            IDecideClientServiceException actualException =
                await Assert.ThrowsAsync<IDecideClientServiceException>(
                    getPatientDecisionsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedServiceException);

            this.decisionOrchestrationServiceMock.Verify(orchestration =>
                orchestration.GetPatientDecisions(),
                    Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
