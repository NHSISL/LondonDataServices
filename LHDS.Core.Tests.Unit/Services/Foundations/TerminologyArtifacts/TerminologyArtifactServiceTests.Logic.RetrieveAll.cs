// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldReturnTerminologyArtifactsAsync()
        {
            // given
            IQueryable<TerminologyArtifact> randomTerminologyArtifacts = CreateRandomTerminologyArtifacts();
            IQueryable<TerminologyArtifact> storageTerminologyArtifacts = randomTerminologyArtifacts;
            IQueryable<TerminologyArtifact> expectedTerminologyArtifacts = storageTerminologyArtifacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(storageTerminologyArtifacts);

            // when
            IQueryable<TerminologyArtifact> actualTerminologyArtifacts =
                await this.terminologyArtifactService.RetrieveAllTerminologyArtifactsAsync();

            // then
            actualTerminologyArtifacts.Should().BeEquivalentTo(expectedTerminologyArtifacts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}