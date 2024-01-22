// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Ontologies;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllConceptMapsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            OntologyAssets createdOntologyAssets = CreateRandomOntologies();
            OntologyAssets expectedOntologyAssets = createdOntologyAssets.DeepClone();

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllConceptMapsAsync(inputRelativeUrl))
                    .ReturnsAsync(createdOntologyAssets);

            // when
            OntologyAssets actualOntologyAssets =
                await this.ontologyProcessingService.RetrieveAllConceptMapsAsync(inputRelativeUrl);

            // then
            actualOntologyAssets.Should().BeEquivalentTo(expectedOntologyAssets);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllConceptMapsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}