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
        public async Task ShouldModifyOntologyValueSetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyValueSet randomOntologyValueSet = CreateRandomModifyOntologyValueSet(randomDateTimeOffset);
            OntologyValueSet inputOntologyValueSet = randomOntologyValueSet;
            OntologyValueSet storageOntologyValueSet = inputOntologyValueSet.DeepClone();
            storageOntologyValueSet.UpdatedDate = randomOntologyValueSet.CreatedDate;
            OntologyValueSet updatedOntologyValueSet = inputOntologyValueSet;
            OntologyValueSet expectedOntologyValueSet = updatedOntologyValueSet.DeepClone();
            Guid ontologyValueSetId = inputOntologyValueSet.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOntologyValueSetAsync(inputOntologyValueSet))
                    .ReturnsAsync(updatedOntologyValueSet);

            // when
            OntologyValueSet actualOntologyValueSet =
                await this.ontologyValueSetService.ModifyOntologyValueSetAsync(inputOntologyValueSet);

            // then
            actualOntologyValueSet.Should().BeEquivalentTo(expectedOntologyValueSet);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyValueSetAsync(inputOntologyValueSet),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}