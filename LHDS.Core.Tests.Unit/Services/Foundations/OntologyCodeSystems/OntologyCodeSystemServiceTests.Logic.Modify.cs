using System;
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
        public async Task ShouldModifyOntologyCodeSystemAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomModifyOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem inputOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem storageOntologyCodeSystem = inputOntologyCodeSystem.DeepClone();
            storageOntologyCodeSystem.UpdatedDate = randomOntologyCodeSystem.CreatedDate;
            OntologyCodeSystem updatedOntologyCodeSystem = inputOntologyCodeSystem;
            OntologyCodeSystem expectedOntologyCodeSystem = updatedOntologyCodeSystem.DeepClone();
            Guid ontologyCodeSystemId = inputOntologyCodeSystem.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOntologyCodeSystemAsync(inputOntologyCodeSystem))
                    .ReturnsAsync(updatedOntologyCodeSystem);

            // when
            OntologyCodeSystem actualOntologyCodeSystem =
                await this.ontologyCodeSystemService.ModifyOntologyCodeSystemAsync(inputOntologyCodeSystem);

            // then
            actualOntologyCodeSystem.Should().BeEquivalentTo(expectedOntologyCodeSystem);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOntologyCodeSystemAsync(inputOntologyCodeSystem),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}