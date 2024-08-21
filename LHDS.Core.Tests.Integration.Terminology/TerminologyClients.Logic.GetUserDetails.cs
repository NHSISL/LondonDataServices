// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyClients
    {
        [Fact]
        public async Task ShouldRetrieveUserArtifactDetailsAsync()
        {
            //Given

            //When
            await terminologyClient.RetrieveUserArtifactDetailsAsync();

            //Then
        }
    }
}
