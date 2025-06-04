// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldHandshake()
        {
            // Given
            // When
            bool canHandshake =
                await pdsClient.ValidateMailboxAccessAsync();

            //Then
            await Task.CompletedTask;
        }
    }
}
