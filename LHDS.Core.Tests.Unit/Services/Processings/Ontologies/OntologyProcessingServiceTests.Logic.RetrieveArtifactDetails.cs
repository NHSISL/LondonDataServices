// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
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

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(inputRelativeUrl))
                    .ReturnsAsync(outputArtifactDetail);

            // when
            string actualArtifactDetail =
                await this.ontologyProcessingService.RetrieveArtifactDetailsAsync(inputRelativeUrl);

            // then
            actualArtifactDetail.Should().BeEquivalentTo(expectedArtifactDetail);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}