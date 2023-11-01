using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveOntologyConceptMapByIdAsync()
        {
            // given
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap();
            OntologyConceptMap inputOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap storageOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap expectedOntologyConceptMap = storageOntologyConceptMap.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(inputOntologyConceptMap.Id))
                    .ReturnsAsync(storageOntologyConceptMap);

            // when
            OntologyConceptMap actualOntologyConceptMap =
                await this.ontologyConceptMapService.RetrieveOntologyConceptMapByIdAsync(inputOntologyConceptMap.Id);

            // then
            actualOntologyConceptMap.Should().BeEquivalentTo(expectedOntologyConceptMap);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(inputOntologyConceptMap.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}