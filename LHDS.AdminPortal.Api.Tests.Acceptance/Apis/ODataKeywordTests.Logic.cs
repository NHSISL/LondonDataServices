// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ApiSupplier = LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers.Supplier;
using CoreSupplier = LHDS.Core.Models.Foundations.Suppliers.Supplier;
using Xunit;
using CoreIngestionTracking = LHDS.Core.Models.Foundations.IngestionTrackings.IngestionTracking;
using ApiIngestionTracking = LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings.IngestionTracking;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Suppliers
{
    public partial class SuppliersApiTests
    {
        [Fact]
        public async Task ShouldGetAllSuppliersThroughOdataAsync()
        {
            //Given
            List<ApiSupplier> randomSuppliers = await PostRandomSuppliersAsync();
            List<ApiSupplier> expectedSuppliers = randomSuppliers;

            //When
            List<CoreSupplier> actualSuppliers = 
                await this.apiBroker.GetAllItemsAsync<CoreSupplier>("odata/suppliers");

            //Then
            foreach (CoreSupplier actualSupplier in actualSuppliers)
            {
                expectedSuppliers.Any(supplier => supplier.Id == actualSupplier.Id).Should().BeTrue();
            }

            foreach (ApiSupplier expectedSupplier in expectedSuppliers)
            {
                await this.apiBroker.DeleteSupplierByIdAsync(expectedSupplier.Id);
            }
        }

        [Fact]
        public async Task ShouldGetAllSuppliersOrderByDescendingThroughOdataAsync()
        {
            //Given
            List<ApiSupplier> randomSuppliers = await PostRandomSuppliersAsync();
            List<ApiSupplier> expectedSuppliers = randomSuppliers;

            //When
            List<CoreSupplier> actualSuppliers =
                await this.apiBroker.GetAllItemsAsync<CoreSupplier>("odata/suppliers?$orderby=createddate desc");

            //Then
            for (int i = 1; i < actualSuppliers.Count; i++)
            {
                (actualSuppliers[i - 1].CreatedDate >= actualSuppliers[i].CreatedDate).Should().BeTrue();
            }

            foreach (ApiSupplier expectedSupplier in expectedSuppliers)
            {
                await this.apiBroker.DeleteSupplierByIdAsync(expectedSupplier.Id);
            }
        }

        [Fact]
        public async Task ShouldFilterSuppliersAsync()
        {
            // given
            List<ApiSupplier> namedSuppliers = await PostRandomSuppliersAsync();
            ApiSupplier expectedSupplier = namedSuppliers.First();
            string supplierName = expectedSupplier.Name;

            // when
            List<CoreSupplier> actualSuppliers =
                await this.apiBroker.GetAllItemsAsync<CoreSupplier>(
                    $"odata/suppliers/?$filter=name eq '{supplierName}'");

            CoreSupplier actualSupplier = actualSuppliers.First();

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            // cleanup
            foreach (ApiSupplier supplier in namedSuppliers)
            {
                await this.apiBroker.DeleteSupplierByIdAsync(supplier.Id);
            }
        }

        [Fact]
        public async Task ShouldGetAllSupplierIngestionTrackingExpandsAsync()
        {
            // given
            ApiSupplier randomSupplier = await PostRandomSupplierAsync();
            ApiSupplier expectedSupplier = randomSupplier;

            List<ApiIngestionTracking> randomIngestionTrackings = 
                await PostRandomIngestionTrackingsAsync(randomSupplier.Id);

            List<ApiIngestionTracking> expectedIngestionTrackings = randomIngestionTrackings;

            // when
            List<CoreSupplier> actualSuppliers =
                await this.apiBroker.GetAllItemsAsync<CoreSupplier>(
                    $"odata/suppliers/?$filter=Id eq {randomSupplier.Id}&$expand=ingestiontrackings");

            // then
            CoreSupplier actualSupplier = actualSuppliers.First();
            actualSupplier.Id.Should().Be(expectedSupplier.Id);

            actualSupplier.IngestionTrackings.Count().Should().
                BeGreaterThanOrEqualTo(expectedIngestionTrackings.Count());

            foreach (CoreIngestionTracking actualIngestionTracking in actualSupplier.IngestionTrackings)
            {
                expectedIngestionTrackings.Any(
                    ingestionTracking => ingestionTracking.Id == actualIngestionTracking.Id).Should().BeTrue();
            }

            foreach (ApiIngestionTracking expectedIngestionTracking in expectedIngestionTrackings)
            {
                await this.apiBroker.DeleteIngestionTrackingByIdAsync(expectedIngestionTracking.Id);
            }

            await this.apiBroker.DeleteSupplierByIdAsync(expectedSupplier.Id);
        }
    }
}