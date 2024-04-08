// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Pds
{
    public partial class TerminologyClients
    {
        [Theory]
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
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
