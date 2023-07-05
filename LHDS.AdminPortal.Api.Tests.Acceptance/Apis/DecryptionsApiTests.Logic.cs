// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Xunit;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis
{
    public partial class DecryptionsApiTests
    {
        
        [Fact]
        public async Task ShouldDecryptFileAsync()
        {
            //Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IngestionTracking randomIngestionTracking = await PostRandomIngestionTrackingAsync(randomSupplier.Id);
            await DeleteAuditRecordsAsync(randomIngestionTracking);
            string randomFileName = GetRandomString();

            IngestionTracking ingestionTracking = CreateRandomIngestionTracking(
                dateTimeOffset: this.dateTimeBroker.GetCurrentDateTimeOffset(),
                document,
                supplierId: this.landingConfiguration.LandingSupplierId);

            await this.apiBroker.PostIngestionTrackingAsync(ingestionTracking);

            //When
            await this.apiBroker.DecryptFileAsync(randomFileName);

            //Then
            bool isDecrypted = await this.apiBroker.IsFileDecryptedAsync(randomFileName);

            isDecrypted.Should().BeTrue();

            var audits = this.apiBroker.RetrieveAllAudits()
                .Where(audit => audit.IngestionTrackingId == ingestionTracking.Id);

            foreach (var audit in audits)
            {
                await this.apiBroker.RemoveAuditByIdAsync(audit.Id);
            }

            IngestionTracking decryptedIngestionTracking =
                await this.apiBroker.RetrieveIngestionTrackingByIdAsync(ingestionTracking.Id);

            await this.apiBroker.RemoveIngestionTrackingByIdAsync(ingestionTracking.Id);
        }
    }
}
