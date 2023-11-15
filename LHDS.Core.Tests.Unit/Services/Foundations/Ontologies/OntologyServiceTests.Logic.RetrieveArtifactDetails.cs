// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologys
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            string randomArtifactDetail = GetRandomString();
            string outputArtifactDetail = randomArtifactDetail;
            string expectedArtifactDetail = outputArtifactDetail.DeepClone();

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetArtifactDetailsAsync(inputRelativeUrl))
                    .ReturnsAsync(outputArtifactDetail);

            // when
            string actualArtifactDetail =
                await this.ontologyService.RetrieveArtifactDetailsAsync(inputRelativeUrl);

            // then
            actualArtifactDetail.Should().BeEquivalentTo(expectedArtifactDetail);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetArtifactDetailsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}