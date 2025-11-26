// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Decisions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.IDecide
{
    public partial class IDecideTests
    {
        [Fact(Skip = "Test temporarily disabled due to environment credential issues. DH to FIX")]
        public async Task ShouldGetPatientDecisions()
        {
            // given
            List<Decision> randomDecisions = CreateRandomDecisions();
            List<Decision> decisions = randomDecisions;
            List<Decision> expectedDecisions = decisions.DeepClone();

            this.wireMockServer
                .Given(
                    Request
                        .Create()
                        .UsingGet()
                        .WithPath(this.decisionConfiguration.IDecidePatientDecisionsRelativeUrl))

                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithBodyAsJson(decisions));

            this.wireMockServer
                .Given(
                    Request
                        .Create()
                        .UsingPost()
                        .WithPath(this.decisionConfiguration.IDecideRecordAdoptionRelativeUrl))

                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(HttpStatusCode.OK));

            // when
            List<Decision> actualDecisions = await this.iDecideClient.GetPatientDecisions();

            // then
            actualDecisions.Should().NotBeNull();
            this.compareLogic.Compare(expectedDecisions, actualDecisions).AreEqual.Should().BeTrue();
        }
    }
}

