// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyTests
    {
        [Theory]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async Task ShouldRetrieveArtifactMetadataAsync(string resourceType)
        {
            //Given
            string[] resourceTypes = new string[] { resourceType };

            //When
            await this.terminologyClient.RetrieveArtifactMetadataAsync(resourceTypes);

            //Then
            IQueryable<TerminologyPoll> retrievedTerminologyPolls =
                this.terminologyPollService.RetrieveAllTerminologyPolls();

            retrievedTerminologyPolls.Count().Should().BeGreaterThan(0);

            foreach (TerminologyPoll poll in retrievedTerminologyPolls)
            {
                await this.terminologyPollService.RemoveTerminologyPollByIdAsync(poll.Id);
            }

            IQueryable<TerminologyArtifact> retrievedTerminologyArtifacts =
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            retrievedTerminologyArtifacts.Count().Should().BeGreaterThan(0);

            foreach (TerminologyArtifact artifact in retrievedTerminologyArtifacts)
            {
                await this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(artifact.Id);
            }
        }
    }
}
