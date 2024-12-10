// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using LHDS.Core.Extensions.Exceptions;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackings
{
    public partial class IngestionTrackingsApiTests
    {
        [Fact]
        public async Task ShouldPostIngestionTrackingAsync()
        {
            try
            {
                // given
                Supplier randomSupplier = await PostRandomSupplierAsync();
                IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomSupplier.Id);
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
                await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
            }
            catch (System.Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}, Validation: {ex.GetValidationSummary()}");
                throw;
            }
        }

        [Fact]
        public async Task ShouldGetAllIngestionTrackingsAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();

            List<IngestionTracking> randomIngestionTrackings =
                await PostRandomIngestionTrackingsAsync(randomSupplier.Id);

            List<IngestionTracking> expectedIngestionTrackings = randomIngestionTrackings;

            // when
            List<IngestionTracking> actualIngestionTrackings = await this.apiBroker.GetAllIngestionTrackingsAsync();

            // then
            foreach (IngestionTracking expectedIngestionTracking in expectedIngestionTrackings)
            {
                IngestionTracking actualIngestionTracking =
                    actualIngestionTrackings.FirstOrDefault(ingestionTracking =>
                        ingestionTracking.Id == expectedIngestionTracking.Id);

                actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

                if (actualIngestionTracking != null)
                {
                    await DeleteAuditRecordsAsync(actualIngestionTracking);
                    await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
                }
            }

            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetIngestionTrackingAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            IngestionTracking expectedIngestionTracking = randomIngestionTracking;

            // when
            IngestionTracking actualIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            await DeleteAuditRecordsAsync(actualIngestionTracking);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutIngestionTrackingAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);

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
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteIngestionTrackingAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
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
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getIngestionTrackingbyIdTask.AsTask);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        private async Task DeleteAuditRecordsAsync(IngestionTracking inputIngestionTracking)
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