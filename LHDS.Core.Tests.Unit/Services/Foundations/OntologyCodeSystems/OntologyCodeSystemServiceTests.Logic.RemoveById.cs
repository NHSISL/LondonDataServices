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
        public async Task ShouldRemoveOntologyCodeSystemByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOntologyCodeSystemId = randomId;
            OntologyCodeSystem randomOntologyCodeSystem = CreateRandomOntologyCodeSystem();
            OntologyCodeSystem storageOntologyCodeSystem = randomOntologyCodeSystem;
            OntologyCodeSystem expectedInputOntologyCodeSystem = storageOntologyCodeSystem;
            OntologyCodeSystem deletedOntologyCodeSystem = expectedInputOntologyCodeSystem;
            OntologyCodeSystem expectedOntologyCodeSystem = deletedOntologyCodeSystem.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(inputOntologyCodeSystemId))
                    .ReturnsAsync(storageOntologyCodeSystem);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOntologyCodeSystemAsync(expectedInputOntologyCodeSystem))
                    .ReturnsAsync(deletedOntologyCodeSystem);

            // when
            OntologyCodeSystem actualOntologyCodeSystem = await this.ontologyCodeSystemService
                .RemoveOntologyCodeSystemByIdAsync(inputOntologyCodeSystemId);

            // then
            actualOntologyCodeSystem.Should().BeEquivalentTo(expectedOntologyCodeSystem);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(inputOntologyCodeSystemId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyCodeSystemAsync(expectedInputOntologyCodeSystem),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}