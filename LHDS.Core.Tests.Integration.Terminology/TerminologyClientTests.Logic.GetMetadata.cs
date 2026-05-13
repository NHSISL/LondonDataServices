// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Integration.Terminology
{
    public partial class TerminologyClientTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetrieveMetadataAsync()
        {
            //Given
            string[] resourceTypes = new string[] { "CodeSystem", "ValueSet", "ConceptMap" };

            //When
            await terminologyClient.RetrieveArtifactMetadataAsync(resourceTypes);

            //Then
        }
    }
}
