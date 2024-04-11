// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Terminologies
{
    public partial class TerminologyClients
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            //Given

            //When
            await terminologyClient.RetrieveArtifactDetailsAsync();

            //Then
        }
    }
}
