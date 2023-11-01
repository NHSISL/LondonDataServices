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
        public async Task ShouldAddOntologyConceptMapAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OntologyConceptMap randomOntologyConceptMap = CreateRandomOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap inputOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap storageOntologyConceptMap = inputOntologyConceptMap;
            OntologyConceptMap expectedOntologyConceptMap = storageOntologyConceptMap.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOntologyConceptMapAsync(inputOntologyConceptMap))
                    .ReturnsAsync(storageOntologyConceptMap);

            // when
            OntologyConceptMap actualOntologyConceptMap = await this.ontologyConceptMapService
                .AddOntologyConceptMapAsync(inputOntologyConceptMap);

            // then
            actualOntologyConceptMap.Should().BeEquivalentTo(expectedOntologyConceptMap);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyConceptMapAsync(inputOntologyConceptMap),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}