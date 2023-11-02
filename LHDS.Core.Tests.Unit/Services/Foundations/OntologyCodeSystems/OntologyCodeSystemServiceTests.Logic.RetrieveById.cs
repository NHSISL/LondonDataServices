using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOntologyCodeSystemByIdAsync()
        {
            // given
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            OntologyCodeSystem inputOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem storageOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem expectedOntologyCodeSystem = storageOntologyCodeSystem.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(inputOntologyCodeSystem.Id))
                    .ReturnsAsync(storageOntologyCodeSystem);

            // when
            OntologyCodeSystem actualOntologyCodeSystem =
                await this.ontologyCodeSystemService.RetrieveOntologyCodeSystemByIdAsync(inputOntologyCodeSystem.Id);

            // then
            actualOntologyCodeSystem.Should().BeEquivalentTo(expectedOntologyCodeSystem);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(inputOntologyCodeSystem.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}