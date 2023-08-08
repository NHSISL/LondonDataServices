// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using RESTFulSense.Exceptions;
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