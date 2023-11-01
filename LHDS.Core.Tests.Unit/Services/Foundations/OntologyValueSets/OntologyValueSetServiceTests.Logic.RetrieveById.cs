using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOntologyValueSetByIdAsync()
        {
            // given
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet();
            OntologyValueSet inputOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet storageOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet expectedOntologyValueSet = storageOntologyValueSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(inputOntologyValueSet.Id))
                    .ReturnsAsync(storageOntologyValueSet);

            // when
            OntologyValueSet actualOntologyValueSet =
                await this.ontologyValueSetService.RetrieveOntologyValueSetByIdAsync(inputOntologyValueSet.Id);

            // then
            actualOntologyValueSet.Should().BeEquivalentTo(expectedOntologyValueSet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(inputOntologyValueSet.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}