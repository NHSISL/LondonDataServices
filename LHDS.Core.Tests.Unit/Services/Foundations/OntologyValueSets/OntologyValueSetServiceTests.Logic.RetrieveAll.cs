using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public void ShouldReturnOntologyValueSets()
        {
            // given
            IQueryable<OntologyValueSet> randomOntologyValueSets = CreateRandomOntologyValueSets();
            IQueryable<OntologyValueSet> storageOntologyValueSets = randomOntologyValueSets;
            IQueryable<OntologyValueSet> expectedOntologyValueSets = storageOntologyValueSets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyValueSets())
                    .Returns(storageOntologyValueSets);

            // when
            IQueryable<OntologyValueSet> actualOntologyValueSets =
                this.ontologyValueSetService.RetrieveAllOntologyValueSets();

            // then
            actualOntologyValueSets.Should().BeEquivalentTo(expectedOntologyValueSets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyValueSets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}