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

namespace LHDS.Core.Tests.Unit.Clients.IDecide
{
    public partial class IDecideClientTests
    {
        [Fact]
        public async Task ShouldGetPatientDecisions()
        {
            // given
            List<Decision> randomDecisions = CreateRandomDecisions();
            List<Decision> expectedDecisions = randomDecisions.DeepClone();

            this.decisionOrchestrationServiceMock.Setup(orchestration =>
                orchestration.GetPatientDecisions())
                    .ReturnsAsync(expectedDecisions);

            // when
            List<Decision> actualDecisions = await this.iDecideClient.GetPatientDecisions();

            // then
            actualDecisions.Should().NotBeNullOrEmpty();
            actualDecisions.Should().BeEquivalentTo(expectedDecisions);

            this.decisionOrchestrationServiceMock.Verify(orchestration =>
                orchestration.GetPatientDecisions(),
                    Times.Once);

            this.decisionOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
