// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Suppliers
{
    public partial class SuppliersApiTests
    {
        [Fact]
        public async Task ShouldFilterSuppliersAsync()
        {
            // given
            List<Supplier> namedSuppliers = await PostRandomSuppliersAsync();
            Supplier expectedSupplier = namedSuppliers.First();

            // when
            List<Supplier> actualSuppliers =
                await this.apiBroker.FilterSuppliersAsync(supplierName: expectedSupplier.Name);

            Supplier actualSupplier = actualSuppliers.First();

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);

            // cleanup
            foreach (Supplier supplier in namedSuppliers)
            {
                await this.apiBroker.DeleteSupplierByIdAsync(supplier.Id);
            }
        }

        [Fact]
        public async Task ShouldGetAllSuppliersOrderAsync()
        {
            //Given
            List<Supplier> randomSuppliers = await PostRandomSuppliersAsync();
            List<Supplier> expectedSuppliers = randomSuppliers;

            //When
            List<Supplier> actualSuppliers = await this.apiBroker.GetAllSuppliersOrderedDescendingAsync();

            //Then
            for (int i = 1; i < actualSuppliers.Count; i++)
            {
                (actualSuppliers[i - 1].CreatedDate >= actualSuppliers[i].CreatedDate).Should().BeTrue();
            }

            foreach (Supplier expectedSupplier in expectedSuppliers)
            {
                await this.apiBroker.DeleteSupplierByIdAsync(expectedSupplier.Id);
            }
        }

        [Fact]
        public async Task ShouldGetAllSupplierIngestionTrackingExpandsAsync()
        {
            // given
            List<Supplier> randomSuppliers = new List<Supplier> { await PostRandomSupplierAsync() };

            foreach (Supplier supplier in randomSuppliers)
            {
                List<IngestionTracking> randomIngestionTrackings = await PostRandomIngestionTrackingsAsync(supplier.Id);

                List<IngestionTracking> orderedIngestionTracking =
                    randomIngestionTrackings.OrderBy(ingestionTracking => ingestionTracking.CreatedDate).ToList();

                supplier.IngestionTrackings.AddRange(orderedIngestionTracking);
            }

            List<Supplier> expectedSuppliers =
                randomSuppliers.OrderBy(supplier => supplier.CreatedDate).ToList();

            // when
            List<Supplier> actualSuppliers = await this.apiBroker.GetAllSupplierIngestionTrackingExpandsAsync();

            // then
            foreach (Supplier expectedSupplier in expectedSuppliers)
            {
                Supplier actualSupplier = actualSuppliers.Single(actual => actual.Id == expectedSupplier.Id);
                actualSupplier.IngestionTrackings.Count().Should().Be(expectedSupplier.IngestionTrackings.Count());

                foreach (IngestionTracking ingestionTracking in actualSupplier.IngestionTrackings)
                {
                    await this.apiBroker.DeleteIngestionTrackingByIdAsync(ingestionTracking.Id);
                }

                await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
            }
        }
    }
}