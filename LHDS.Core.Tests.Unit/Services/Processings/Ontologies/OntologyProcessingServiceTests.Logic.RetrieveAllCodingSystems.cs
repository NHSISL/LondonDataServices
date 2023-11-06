// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllCodingSystemsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            string nextPageUrl = "http://localhost:5000/api/fhir/ValueSet?_page=2";
            string artifactType = "CodeSystem";

            List<dynamic> randomArtifactProperties = CreateRandomArtifactProperties(artifactType);
            OntologyAssets createdOntologyAssets = CreateArtiFactFromRandomData(randomArtifactProperties, nextPageUrl);
            OntologyAssets expectedOntologyAssets = CreateArtiFactFromRandomData(randomArtifactProperties, nextPageUrl);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(inputRelativeUrl))
                    .ReturnsAsync(expectedOntologyAssets);

            // when
            OntologyAssets actualOntologyAssets =
                await this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(inputRelativeUrl);

            // then
            actualOntologyAssets.Should().BeEquivalentTo(expectedOntologyAssets);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllCodingSystemsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}