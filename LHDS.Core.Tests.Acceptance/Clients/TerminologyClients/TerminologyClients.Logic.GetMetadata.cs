// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyClients
    {
        [Fact(Skip = "Add wiremock to this test and move duplicate test as is to integration tests")]
        public async Task ShouldRetrieveMetadataAsync()
        {
            //Given

            //When
            await terminologyClient.RetrieveArtifactMetadataAsync();

            //Then
        }
    }
}
