using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public void ShouldReturnOntologyCodeSystems()
        {
            // given
            IQueryable<OntologyCodeSystem> randomOntologyCodeSystems = CreateRandomOntologyCodeSystems();
            IQueryable<OntologyCodeSystem> storageOntologyCodeSystems = randomOntologyCodeSystems;
            IQueryable<OntologyCodeSystem> expectedOntologyCodeSystems = storageOntologyCodeSystems;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyCodeSystems())
                    .Returns(storageOntologyCodeSystems);

            // when
            IQueryable<OntologyCodeSystem> actualOntologyCodeSystems =
                this.ontologyCodeSystemService.RetrieveAllOntologyCodeSystems();

            // then
            actualOntologyCodeSystems.Should().BeEquivalentTo(expectedOntologyCodeSystems);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyCodeSystems(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}