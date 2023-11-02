using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public void ShouldReturnOntologyConceptMaps()
        {
            // given
            IQueryable<OntologyConceptMap> randomOntologyConceptMaps = CreateRandomOntologyConceptMaps();
            IQueryable<OntologyConceptMap> storageOntologyConceptMaps = randomOntologyConceptMaps;
            IQueryable<OntologyConceptMap> expectedOntologyConceptMaps = storageOntologyConceptMaps;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyConceptMaps())
                    .Returns(storageOntologyConceptMaps);

            // when
            IQueryable<OntologyConceptMap> actualOntologyConceptMaps =
                this.ontologyConceptMapService.RetrieveAllOntologyConceptMaps();

            // then
            actualOntologyConceptMaps.Should().BeEquivalentTo(expectedOntologyConceptMaps);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyConceptMaps(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}