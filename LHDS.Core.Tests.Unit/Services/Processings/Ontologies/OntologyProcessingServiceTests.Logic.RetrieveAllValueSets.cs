// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldRetrieveAllValueSetsByRelativeUrlAsync()
        {
            // given
            string randomRelativeUrl = GetRandomString();
            string inputRelativeUrl = randomRelativeUrl;
            OntologyAssets createdOntologyAssets = CreateRandomOntologies();
            OntologyAssets expectedOntologyAssets = createdOntologyAssets.DeepClone();

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllValueSetsAsync(inputRelativeUrl))
                    .ReturnsAsync(expectedOntologyAssets);

            // when
            OntologyAssets actualOntologyAssets =
                await this.ontologyProcessingService.RetrieveAllValueSetsAsync(inputRelativeUrl);

            // then
            actualOntologyAssets.Should().BeEquivalentTo(expectedOntologyAssets);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllValueSetsAsync(inputRelativeUrl),
                    Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}