// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyClients
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetrieveUserArtifactDetailsAsync()
        {
            //Given

            //When
            await terminologyClient.RetrieveUserArtifactDetailsAsync();

            //Then
        }
    }
}
