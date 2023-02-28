// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    public partial class AuditsApiTests
    {
        [Fact]
        public async Task ShouldPostAuditAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = CreateRandomAudit(randomIngestionTracking.FileName);
            Audit inputAudit = randomAudit;
            Audit expectedAudit = inputAudit;

            // when
            await this.apiBroker.PostAuditAsync(inputAudit);

            Audit actualAudit =
                await this.apiBroker.GetAuditByIdAsync(inputAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.FileName);
        }

        [Fact]
        public async Task ShouldGetAllAuditsAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            List<Audit> randomAudits = await PostRandomAuditsAsync(randomIngestionTracking.FileName);
            List<Audit> expectedAudits = randomAudits;

            // when
            List<Audit> actualAudits = await this.apiBroker.GetAllAuditsAsync();

            // then
            foreach (Audit expectedAudit in expectedAudits)
            {
                Audit actualAudit = actualAudits.Single(approval => approval.Id == expectedAudit.Id);
                actualAudit.Should().BeEquivalentTo(expectedAudit);
                await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            }

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.FileName);
        }

        [Fact]
        public async Task ShouldGetAuditAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.FileName);
            Audit expectedAudit = randomAudit;

            // when
            Audit actualAudit = await this.apiBroker.GetAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.FileName);
        }

        [Fact]
        public async Task ShouldPutAuditAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.FileName);
            Audit modifiedAudit = UpdateAuditWithRandomValues(randomAudit);

            // when
            await this.apiBroker.PutAuditAsync(modifiedAudit);
            Audit actualAudit = await this.apiBroker.GetAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(modifiedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.FileName);
        }

        [Fact]
        public async Task ShouldDeleteAuditAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.FileName);
            Audit inputAudit = randomAudit;
            Audit expectedAudit = inputAudit;

            // when
            Audit deletedAudit =
                await this.apiBroker.DeleteAuditByIdAsync(inputAudit.Id);

            ValueTask<Audit> getAuditbyIdTask =
                this.apiBroker.GetAuditByIdAsync(inputAudit.Id);

            // then
            deletedAudit.Should().BeEquivalentTo(expectedAudit);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getAuditbyIdTask.AsTask());

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.FileName);
        }

        private async Task DeleteAuditRecordsAsync(IngestionTracking inputIngestionTracking)
        {
            var audits = this.apiBroker.auditService.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == inputIngestionTracking.FileName);

            foreach (var audit in audits)
            {
                await this.apiBroker.DeleteAuditByIdAsync(audit.Id);
            }
        }
    }
}