// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyClients
    {
        [Theory(Skip = "Add wiremock to this test and move duplicate test as is to integration tests")]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async Task ShouldRetrieveMetadataAsync(string inputResourceType)
        {
            //Given
            string resourceType = inputResourceType;

            //When
            await terminologyClient.RetrieveArtifactMetadataAsync(resourceType);

            //Then
        }
    }
}
