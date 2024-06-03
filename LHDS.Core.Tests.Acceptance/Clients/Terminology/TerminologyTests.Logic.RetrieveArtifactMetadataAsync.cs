// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminology
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
        }
    }
}
