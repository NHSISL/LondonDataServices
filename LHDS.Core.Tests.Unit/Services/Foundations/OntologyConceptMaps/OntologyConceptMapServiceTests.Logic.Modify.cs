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
        public async Task ShouldModifyOntologyConceptMapAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyConceptMap randomOntologyConceptMap = CreateRandomModifyOntologyConceptMap(randomDateTimeOffset);
            OntologyConceptMap inputOntologyConceptMap = randomOntologyConceptMap;
            OntologyConceptMap storageOntologyConceptMap = inputOntologyConceptMap.DeepClone();
            storageOntologyConceptMap.UpdatedDate = randomOntologyConceptMap.CreatedDate;
            OntologyConceptMap updatedOntologyConceptMap = inputOntologyConceptMap;
            OntologyConceptMap expectedOntologyConceptMap = updatedOntologyConceptMap.DeepClone();
            Guid ontologyConceptMapId = inputOntologyConceptMap.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOntologyConceptMapAsync(inputOntologyConceptMap))
                    .ReturnsAsync(updatedOntologyConceptMap);

            // when
            OntologyConceptMap actualOntologyConceptMap =
                await this.ontologyConceptMapService.ModifyOntologyConceptMapAsync(inputOntologyConceptMap);

            // then
            actualOntologyConceptMap.Should().BeEquivalentTo(expectedOntologyConceptMap);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyConceptMapAsync(inputOntologyConceptMap),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}