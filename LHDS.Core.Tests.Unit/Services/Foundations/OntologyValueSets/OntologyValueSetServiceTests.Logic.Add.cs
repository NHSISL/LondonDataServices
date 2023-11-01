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
        public async Task ShouldAddOntologyValueSetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet inputOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet storageOntologyValueSet = inputOntologyValueSet;
            OntologyValueSet expectedOntologyValueSet = storageOntologyValueSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOntologyValueSetAsync(inputOntologyValueSet))
                    .ReturnsAsync(storageOntologyValueSet);

            // when
            OntologyValueSet actualOntologyValueSet = await this.ontologyValueSetService
                .AddOntologyValueSetAsync(inputOntologyValueSet);

            // then
            actualOntologyValueSet.Should().BeEquivalentTo(expectedOntologyValueSet);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyValueSetAsync(inputOntologyValueSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}