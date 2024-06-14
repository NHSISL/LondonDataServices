// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyTests
    {
        [Fact]
        public async Task ShouldRetrieveArtifactDetailsAsync()
        {
            //Given

            //When
            await this.terminologyClient.RetrieveArtifactDetailsAsync();

            //Then
        }
    }
}
