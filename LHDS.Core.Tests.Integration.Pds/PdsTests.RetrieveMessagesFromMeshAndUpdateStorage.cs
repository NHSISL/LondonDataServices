// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Tests.Integration.Pds
{
    public partial class PdsTests
    {
        [ReleaseCandidateFact]
        public async Task ShouldRetrieveMessagesFromMeshAndUpdateStorageAsync()
        {
            // Given

            // When
            List<PdsAudit> actualPdsAudits =
               await this.pdsClient.RetreiveMessagesFromMeshAndUpdateStorage();

            // Then
            actualPdsAudits.Should().NotBeNull();
        }
    }
}
