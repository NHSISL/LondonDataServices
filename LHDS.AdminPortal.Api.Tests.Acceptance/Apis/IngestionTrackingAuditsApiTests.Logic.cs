// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackingAudits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Extensions.Exceptions;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditsApiTests
    {
        [Fact]
        public async Task ShouldPostAuditAsync()
        {
            try
            {
                // given
                Supplier randomSupplier = await PostRandomSupplierAsync();
                IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
                await DeleteIngestionTrackingAuditRecordsAsync(randomIngestionTracking);
                IngestionTrackingAudit randomAudit = CreateRandomIngestionTrackingAudit(randomIngestionTracking.Id);
                IngestionTrackingAudit inputAudit = randomAudit;
                IngestionTrackingAudit expectedAudit = inputAudit;

                // when
                await this.apiBroker.PostIngestionTrackingAuditAsync(inputAudit);

                IngestionTrackingAudit actualAudit =
                    await this.apiBroker.GetIngestionTrackingAuditByIdAsync(inputAudit.Id);

                // then
                actualAudit.Should().BeEquivalentTo(expectedAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(actualAudit.Id);
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
            await DeleteIngestionTrackingAuditRecordsAsync(randomIngestionTracking);
            List<IngestionTrackingAudit> randomAudits = await PostRandomIngestionTrackingAuditsAsync(randomIngestionTracking.Id);
            List<IngestionTrackingAudit> expectedAudits = randomAudits;

            // when
            List<IngestionTrackingAudit> actualAudits = await this.apiBroker.GetAllIngestionTrackingAuditsAsync();

            // then
            foreach (IngestionTrackingAudit expectedAudit in expectedAudits)
            {
                IngestionTrackingAudit actualAudit = actualAudits.Single(approval => approval.Id == expectedAudit.Id);

                actualAudit.Should().BeEquivalentTo(expectedAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(actualAudit.Id);
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
            await DeleteIngestionTrackingAuditRecordsAsync(randomIngestionTracking);
            IngestionTrackingAudit randomAudit = await PostRandomIngestionTrackingAuditAsync(randomIngestionTracking.Id);
            IngestionTrackingAudit expectedAudit = randomAudit;

            // when
            IngestionTrackingAudit actualAudit = await this.apiBroker.GetIngestionTrackingAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutAuditAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteIngestionTrackingAuditRecordsAsync(randomIngestionTracking);
            IngestionTrackingAudit randomAudit = await PostRandomIngestionTrackingAuditAsync(randomIngestionTracking.Id);
            IngestionTrackingAudit modifiedAudit = UpdateIngestionTrackingAuditWithRandomValues(randomAudit);

            // when
            await this.apiBroker.PutIngestionTrackingAuditAsync(modifiedAudit);

            IngestionTrackingAudit actualAudit = 
                await this.apiBroker.GetIngestionTrackingAuditByIdAsync(randomAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(modifiedAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(actualAudit.Id);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteAuditAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteIngestionTrackingAuditRecordsAsync(randomIngestionTracking);
            IngestionTrackingAudit randomAudit = await PostRandomIngestionTrackingAuditAsync(randomIngestionTracking.Id);
            IngestionTrackingAudit inputAudit = randomAudit;
            IngestionTrackingAudit expectedAudit = inputAudit;

            // when
            IngestionTrackingAudit deletedAudit =
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(inputAudit.Id);

            ValueTask<IngestionTrackingAudit> getAuditbyIdTask =
                this.apiBroker.GetIngestionTrackingAuditByIdAsync(inputAudit.Id);

            // then
            deletedAudit.Should().BeEquivalentTo(expectedAudit, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getAuditbyIdTask.AsTask);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(randomIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        private async ValueTask DeleteIngestionTrackingAuditRecordsAsync(IngestionTracking inputIngestionTracking)
        {
            var ingestionTrackingAudits = await this.apiBroker
                .FindIngestionTrackingAuditByIngestionTrackingIdAsync(inputIngestionTracking.Id);

            foreach (var ingestionTrackingAudit in ingestionTrackingAudits)
            {
                await this.apiBroker.DeleteIngestionTrackingAuditByIdAsync(ingestionTrackingAudit.Id);
            }
        }
    }
}