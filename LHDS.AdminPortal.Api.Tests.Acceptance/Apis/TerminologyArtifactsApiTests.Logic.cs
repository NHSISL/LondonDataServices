using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ShouldGetAllTerminologyArtifactsAsync()
        {
            // given
            List<TerminologyArtifact> randomTerminologyArtifacts = await PostRandomTerminologyArtifactsAsync();
            List<TerminologyArtifact> expectedTerminologyArtifacts = randomTerminologyArtifacts;

            // when
            List<TerminologyArtifact> actualTerminologyArtifacts = await this.apiBroker.GetAllTerminologyArtifactsAsync();

            // then
            foreach (TerminologyArtifact expectedTerminologyArtifact in expectedTerminologyArtifacts)
            {
                TerminologyArtifact actualTerminologyArtifact = actualTerminologyArtifacts.Single(approval => approval.Id == expectedTerminologyArtifact.Id);
                actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);
                await this.apiBroker.DeleteTerminologyArtifactByIdAsync(actualTerminologyArtifact.Id);
            }
        }

        [Fact]
        public async Task ShouldGetTerminologyArtifactAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = await PostRandomTerminologyArtifactAsync();
            TerminologyArtifact expectedTerminologyArtifact = randomTerminologyArtifact;

            // when
            TerminologyArtifact actualTerminologyArtifact = await this.apiBroker.GetTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);
            await this.apiBroker.DeleteTerminologyArtifactByIdAsync(actualTerminologyArtifact.Id);
        }
    }
}