// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackings
{
    public partial class IngestionTrackingsApiTests
    {
        [Fact]
        public async Task ShouldPostIngestionTrackingAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            // when 
            await this.apiBroker.PostIngestionTrackingAsync(inputIngestionTracking);

            IngestionTracking actualIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            await DeleteAuditRecordsAsync(actualIngestionTracking);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
        }

        [Fact]
        public async Task ShouldGetAllIngestionTrackingsAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackings = await PostRandomIngestionTrackingsAsync();
            List<IngestionTracking> expectedIngestionTrackings = randomIngestionTrackings;

            // when
            List<IngestionTracking> actualIngestionTrackings = await this.apiBroker.GetAllIngestionTrackingsAsync();

            // then
            foreach (IngestionTracking expectedIngestionTracking in expectedIngestionTrackings)
            {
                IngestionTracking actualIngestionTracking =
                    actualIngestionTrackings.Single(approval =>
                        approval.Id == expectedIngestionTracking.Id);

                actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
                await DeleteAuditRecordsAsync(actualIngestionTracking);
                await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
            }
        }

        [Fact]
        public async Task ShouldGetIngestionTrackingAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            IngestionTracking expectedIngestionTracking = randomIngestionTracking;

            // when
            IngestionTracking actualIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            await DeleteAuditRecordsAsync(actualIngestionTracking);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
        }

        [Fact]
        public async Task ShouldPutIngestionTrackingAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();

            IngestionTracking modifiedIngestionTracking =
                UpdateIngestionTrackingWithRandomValues(randomIngestionTracking);

            // when
            await this.apiBroker.PutIngestionTrackingAsync(modifiedIngestionTracking);

            IngestionTracking actualIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(modifiedIngestionTracking);
            await DeleteAuditRecordsAsync(actualIngestionTracking);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
        }

        [Fact]
        public async Task ShouldDeleteIngestionTrackingAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            // when
            await DeleteAuditRecordsAsync(inputIngestionTracking);

            IngestionTracking deletedIngestionTracking =
                await this.apiBroker.DeleteIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            ValueTask<IngestionTracking> getIngestionTrackingbyIdTask =
                this.apiBroker.GetIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            // then
            deletedIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getIngestionTrackingbyIdTask.AsTask());

        }

        private async Task DeleteAuditRecordsAsync(IngestionTracking inputIngestionTracking)
        {
            var audits = this.apiBroker.auditService.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == inputIngestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.apiBroker.DeleteAuditByIdAsync(audit.Id);
            }
        }
    }
}