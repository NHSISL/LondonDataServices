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
        public async Task ShouldAddOntologyCodeSystemAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem(randomDateTimeOffset);
            OntologyCodeSystem inputOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem storageOntologyCodeSystem = inputOntologyCodeSystem;
            OntologyCodeSystem expectedOntologyCodeSystem = storageOntologyCodeSystem.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOntologyCodeSystemAsync(inputOntologyCodeSystem))
                    .ReturnsAsync(storageOntologyCodeSystem);

            // when
            OntologyCodeSystem actualOntologyCodeSystem = await this.ontologyCodeSystemService
                .AddOntologyCodeSystemAsync(inputOntologyCodeSystem);

            // then
            actualOntologyCodeSystem.Should().BeEquivalentTo(expectedOntologyCodeSystem);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(inputOntologyCodeSystem),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}