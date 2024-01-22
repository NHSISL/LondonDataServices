using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public void ShouldReturnTerminologyArtifacts()
        {
            // given
            IQueryable<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> storageTerminologyArtifacts = randomTerminologyArtifacts;
            IQueryable<TerminologyArtifact> expectedTerminologyArtifacts = storageTerminologyArtifacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifacts())
                    .Returns(storageTerminologyArtifacts);

            // when
            IQueryable<TerminologyArtifact> actualTerminologyArtifacts =
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            // then
            actualTerminologyArtifacts.Should().BeEquivalentTo(expectedTerminologyArtifacts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifacts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}