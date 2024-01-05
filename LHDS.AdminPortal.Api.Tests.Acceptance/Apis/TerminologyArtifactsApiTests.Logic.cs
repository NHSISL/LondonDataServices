using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.TerminologyArtifacts
{
    public partial class TerminologyArtifactsApiTests
    {
        [Fact]
        public async Task ShouldPostTerminologyArtifactAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = inputTerminologyArtifact;

            // when 
            await this.apiBroker.PostTerminologyArtifactAsync(inputTerminologyArtifact);

            TerminologyArtifact actualTerminologyArtifact =
                await this.apiBroker.GetTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);
            await this.apiBroker.DeleteTerminologyArtifactByIdAsync(actualTerminologyArtifact.Id);
        }
    }
}