using System;
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
        public async Task ShouldRemoveOntologyValueSetByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOntologyValueSetId = randomId;
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet();
            OntologyValueSet storageOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet expectedInputOntologyValueSet = storageOntologyValueSet;
            OntologyValueSet deletedOntologyValueSet = expectedInputOntologyValueSet;
            OntologyValueSet expectedOntologyValueSet = deletedOntologyValueSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyValueSetByIdAsync(inputOntologyValueSetId))
                    .ReturnsAsync(storageOntologyValueSet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOntologyValueSetAsync(expectedInputOntologyValueSet))
                    .ReturnsAsync(deletedOntologyValueSet);

            // when
            OntologyValueSet actualOntologyValueSet = await this.ontologyValueSetService
                .RemoveOntologyValueSetByIdAsync(inputOntologyValueSetId);

            // then
            actualOntologyValueSet.Should().BeEquivalentTo(expectedOntologyValueSet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(inputOntologyValueSetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyValueSetAsync(expectedInputOntologyValueSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}