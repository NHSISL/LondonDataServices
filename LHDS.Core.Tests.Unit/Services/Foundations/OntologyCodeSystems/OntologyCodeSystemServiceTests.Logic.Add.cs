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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOntologyCodeSystemAsync(inputOntologyCodeSystem))
                    .ReturnsAsync(storageOntologyCodeSystem);

            // when
            OntologyCodeSystem actualOntologyCodeSystem = await this.ontologyCodeSystemService
                .AddOntologyCodeSystemAsync(inputOntologyCodeSystem);

            // then
            actualOntologyCodeSystem.Should().BeEquivalentTo(expectedOntologyCodeSystem);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOntologyCodeSystemAsync(inputOntologyCodeSystem),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}