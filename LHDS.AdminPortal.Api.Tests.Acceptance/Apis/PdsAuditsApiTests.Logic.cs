// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;

            // when 
            await this.apiBroker.PostPdsAuditAsync(inputPdsAudit);

            PdsAudit actualPdsAudit =
                await this.apiBroker.GetPdsAuditByIdAsync(inputPdsAudit.Id);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);
            await this.apiBroker.DeletePdsAuditByIdAsync(actualPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldGetAllPdsAuditsAsync()
        {
            // given
            List<PdsAudit> randomPdsAudits =
                await PostRandomPdsAuditsAsync();

            List<PdsAudit> expectedPdsAudits = randomPdsAudits;

            // when
            List<PdsAudit> actualPdsAudits = await this.apiBroker.GetAllPdsAuditsAsync();

            // then
            foreach (PdsAudit expectedPdsAudit in expectedPdsAudits)
            {
                PdsAudit actualPdsAudit =
                    actualPdsAudits.Single(approval =>
                        approval.Id == expectedPdsAudit.Id);

                actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);
                await this.apiBroker.DeletePdsAuditByIdAsync(actualPdsAudit.Id);
            }

        }

        [Fact]
        public async Task ShouldGetPdsAuditAsync()
        {
            // given
            PdsAudit randomPdsAudit = await PostRandomPdsAuditAsync();
            PdsAudit expectedPdsAudit = randomPdsAudit;

            // when
            PdsAudit actualPdsAudit =
                await this.apiBroker.GetPdsAuditByIdAsync(randomPdsAudit.Id);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);
            await this.apiBroker.DeletePdsAuditByIdAsync(actualPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldPutPdsAuditAsync()
        {
            // given
            PdsAudit randomPdsAudit = await PostRandomPdsAuditAsync();

            PdsAudit modifiedPdsAudit =
                UpdatePdsAuditWithRandomValues(randomPdsAudit);

            // when
            await this.apiBroker.PutPdsAuditAsync(modifiedPdsAudit);

            PdsAudit actualPdsAudit =
                await this.apiBroker.GetPdsAuditByIdAsync(randomPdsAudit.Id);

            // then
            actualPdsAudit.Should().BeEquivalentTo(modifiedPdsAudit);
            await this.apiBroker.DeletePdsAuditByIdAsync(actualPdsAudit.Id);
        }

        [Fact]
        public async Task ShouldDeletePdsAuditAsync()
        {
            // given
            PdsAudit randomPdsAudit = await PostRandomPdsAuditAsync();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = inputPdsAudit;

            // when

            PdsAudit deletedPdsAudit =
                await this.apiBroker.DeletePdsAuditByIdAsync(inputPdsAudit.Id);

            ValueTask<PdsAudit> getPdsAuditbyIdTask =
                this.apiBroker.GetPdsAuditByIdAsync(inputPdsAudit.Id);

            // then
            deletedPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getPdsAuditbyIdTask.AsTask());

        }
    }
}