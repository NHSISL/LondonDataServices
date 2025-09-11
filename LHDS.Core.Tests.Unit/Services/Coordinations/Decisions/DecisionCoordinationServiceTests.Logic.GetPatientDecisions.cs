// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decisions
{
    public partial class DecisionCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldGetPatientDecisions()
        {
            // given
            List<Decision> randomDecisions = CreateRandomDecisions();
            List<Decision> expectedDecisions = randomDecisions.DeepClone();

            this.decisionOrchestrationServiceMock.Setup(service =>
                service.GetPatientDecisions())
                    .ReturnsAsync(randomDecisions);

            // when
            List<Decision> actualDecision = await this.decisionCoordinationService.GetPatientDecisions();

            // then
            actualDecision.Should().BeEquivalentTo(expectedDecisions);

            this.decisionOrchestrationServiceMock.Verify(service =>
                service.GetPatientDecisions(),
                    Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
