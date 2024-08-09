// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyArtifacts;
using RESTFulSense.Exceptions;
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

        [Fact(Skip = "Will fix in another PR.")]
        public async Task ShouldGetAllTerminologyArtifactsAsync()
        {
            // given
            List<TerminologyArtifact> randomTerminologyArtifacts = await PostRandomTerminologyArtifactsAsync();
            List<TerminologyArtifact> expectedTerminologyArtifacts = randomTerminologyArtifacts;

            // when
            List<TerminologyArtifact> actualTerminologyArtifacts = 
                await this.apiBroker.GetAllTerminologyArtifactsAsync().ToList();

            // then
            foreach (TerminologyArtifact expectedTerminologyArtifact in expectedTerminologyArtifacts)
            {
                TerminologyArtifact actualTerminologyArtifact = actualTerminologyArtifacts.Single(approval => 
                    approval.Id == expectedTerminologyArtifact.Id);

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

        [Fact]
        public async Task ShouldPutTerminologyArtifactAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = await PostRandomTerminologyArtifactAsync();
            TerminologyArtifact modifiedTerminologyArtifact = UpdateTerminologyArtifactWithRandomValues(randomTerminologyArtifact);

            // when
            await this.apiBroker.PutTerminologyArtifactAsync(modifiedTerminologyArtifact);
            TerminologyArtifact actualTerminologyArtifact = await this.apiBroker.GetTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id);

            // then
            actualTerminologyArtifact.Should().BeEquivalentTo(modifiedTerminologyArtifact);
            await this.apiBroker.DeleteTerminologyArtifactByIdAsync(actualTerminologyArtifact.Id);
        }

        [Fact]
        public async Task ShouldDeleteTerminologyArtifactAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = await PostRandomTerminologyArtifactAsync();
            TerminologyArtifact inputTerminologyArtifact = randomTerminologyArtifact;
            TerminologyArtifact expectedTerminologyArtifact = inputTerminologyArtifact;

            // when
            TerminologyArtifact deletedTerminologyArtifact =
                await this.apiBroker.DeleteTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id);

            ValueTask<TerminologyArtifact> getTerminologyArtifactbyIdTask =
                this.apiBroker.GetTerminologyArtifactByIdAsync(inputTerminologyArtifact.Id);

            // then
            deletedTerminologyArtifact.Should().BeEquivalentTo(expectedTerminologyArtifact);
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getTerminologyArtifactbyIdTask.AsTask);
        }
    }
}