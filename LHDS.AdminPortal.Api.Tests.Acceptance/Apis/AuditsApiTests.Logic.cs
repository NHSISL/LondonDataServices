// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Extensions.Exceptions;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    public partial class AuditsApiTests
    {
        [Fact]
        public async Task ShouldPostAuditAsync()
        {
            try
            {
                // given
                Supplier randomSupplier = await PostRandomSupplierAsync();
                IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
                await DeleteAuditRecordsAsync(randomIngestionTracking);
                Audit randomAudit = CreateRandomAudit(randomIngestionTracking.Id);
                Audit inputAudit = randomAudit;
                Audit expectedAudit = inputAudit;

                // when
                await this.apiBroker.PostAuditAsync(inputAudit);

                Audit actualAudit =
                    await this.apiBroker.GetAuditByIdAsync(inputAudit.Id);

                // then
                actualAudit.Should().BeEquivalentTo(expectedAudit);
                await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
                await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
                await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
            }
            catch (System.Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}, Validation: {ex.GetValidationSummary()}");
                throw;
            }

        }

        [Fact]
        public async Task ShouldGetAllAuditsAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            List<Audit> randomAudits = await PostRandomAuditsAsync(randomIngestionTracking.Id);
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

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetAuditAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.Id);
            Audit expectedAudit = randomAudit;

            // when
            Audit actualAudit = await this.apiBroker.GetAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutAuditAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.Id);
            Audit modifiedAudit = UpdateAuditWithRandomValues(randomAudit);

            // when
            await this.apiBroker.PutAuditAsync(modifiedAudit);
            Audit actualAudit = await this.apiBroker.GetAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(modifiedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteAuditAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            Audit randomAudit = await PostRandomAuditAsync(randomIngestionTracking.Id);
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

            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        private async Task DeleteAuditRecordsAsync(IngestionTracking inputIngestionTracking)
        {
            var audits = this.apiBroker.auditService.RetrieveAllIngestionTrackingAudits()
                .Where(audit => audit.IngestionTrackingId == inputIngestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.apiBroker.DeleteAuditByIdAsync(audit.Id);
            }
        }
    }
}