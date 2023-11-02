using System;
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
        public async Task ShouldRemoveOntologyConceptMapByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOntologyConceptMapId = randomId;
            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap();
            OntologyConceptMap storageOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap expectedInputOntologyConceptMap = storageOntologyConceptMap;
            OntologyConceptMap deletedOntologyConceptMap = expectedInputOntologyConceptMap;
            OntologyConceptMap expectedOntologyConceptMap = deletedOntologyConceptMap.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyConceptMapByIdAsync(inputOntologyConceptMapId))
                    .ReturnsAsync(storageOntologyConceptMap);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOntologyConceptMapAsync(expectedInputOntologyConceptMap))
                    .ReturnsAsync(deletedOntologyConceptMap);

            // when
            OntologyConceptMap actualOntologyConceptMap = await this.ontologyConceptMapService
                .RemoveOntologyConceptMapByIdAsync(inputOntologyConceptMapId);

            // then
            actualOntologyConceptMap.Should().BeEquivalentTo(expectedOntologyConceptMap);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyConceptMapByIdAsync(inputOntologyConceptMapId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyConceptMapAsync(expectedInputOntologyConceptMap),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}