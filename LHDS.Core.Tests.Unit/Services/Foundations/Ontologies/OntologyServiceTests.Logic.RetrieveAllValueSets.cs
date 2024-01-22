// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologies
{
    public partial class OntologyServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldRetrieveAllValueSetsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            string nextPageUrl = "http://localhost:5000/api/fhir/ValueSet?_page=2";
            string artifactType = "ValueSet";

            List<dynamic> randomArtifactProperties = CreateRandomArtifactProperties(artifactType);

            var remoteValueSetBundle = CreateValueSetBundleFromRandomData(randomArtifactProperties, nextPageUrl);
            var expectedOntologyAssets = CreateArtiFactFromRandomData(randomArtifactProperties, nextPageUrl);

            this.ontologyBrokerMock.Setup(broker =>
                broker.GetAllValueSetsAsync(inputRelativeUrl))
                    .ReturnsAsync(remoteValueSetBundle);

            // when
            OntologyAssets actualOntologyAssets =
                await this.ontologyService.RetrieveAllValueSetsAsync(inputRelativeUrl);

            // then
            actualOntologyAssets.Should().BeEquivalentTo(expectedOntologyAssets);

            this.ontologyBrokerMock.Verify(broker =>
                broker.GetAllValueSetsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}