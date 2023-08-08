// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.PdsAudits
{
    public partial class PdsAuditsApiTests
    {
        [Fact]
        public async Task ShouldPostPdsAuditAsync()
        {
            // Given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;

            // When
            PdsAudit actualPdsAudit = await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            // Then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            // Cleanup
            await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldGetAllPdsAuditsAsync()
        {
            // Given
            var randomAudits = CreateRandomPdsAudits();

            foreach (var randomAudit in randomAudits)
            {
                await this.apiBroker.PostPdsAuditAsync(randomAudit);
            }

            // When
            List<PdsAudit> actualPdsAudits = await this.apiBroker.GetAllPdsAuditsAsync();

            // Then
            actualPdsAudits.Should().BeEquivalentTo(randomAudits);

            // Cleanup
            foreach (var randomAudit in randomAudits)
            {
                await this.apiBroker.DeletePdsAuditByIdAsync(randomAudit.Id);
            }
        }
    }
}