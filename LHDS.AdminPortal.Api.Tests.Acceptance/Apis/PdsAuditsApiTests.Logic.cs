// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
            IQueryable<PdsAudit> randomPdsAudits = CreateRandomPdsAudits();
            IQueryable<PdsAudit> inputPdsAudits = randomPdsAudits;
            IQueryable<PdsAudit> expectedPdsAudits = inputPdsAudits;

            foreach (PdsAudit inputPdsAudit in inputPdsAudits)
            {
                await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);
            }

            // When
            List<PdsAudit> actualPdsAudits = await this.apiBroker.GetAllPdsAuditsAsync();

            // Then
            actualPdsAudits.Should().BeEquivalentTo(inputPdsAudits);

            // Cleanup
            foreach (PdsAudit inputPdsAudit in inputPdsAudits)
            {
                await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);
            }
          
        [Fact]
        public async Task ShouldGetPdsAuditByIdAsync()
        {
            // Given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;
            await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            // When
            PdsAudit actualPdsAudit = 
                await this.apiBroker.GetPdsAuditByIdAsync(inputPdsAudit.Id);

            // Then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            // Cleanup
            await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);
        }
    }
}