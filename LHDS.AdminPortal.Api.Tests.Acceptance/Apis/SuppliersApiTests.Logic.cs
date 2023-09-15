// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Microsoft.AspNetCore.Http;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Suppliers
{
    public partial class SuppliersApiTests
    {
        [Fact]
        public async Task ShouldPostSupplierAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            Supplier inputSupplier = randomSupplier;
            Supplier expectedSupplier = inputSupplier;

            // when 
            await this.apiBroker.PostSupplierAsync(inputSupplier);

            Supplier actualSupplier =
                await this.apiBroker.GetSupplierByIdAsync(inputSupplier.Id);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);
            await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetAllSuppliersAsync()
        {
            // given
            List<Supplier> randomSuppliers = await PostRandomSuppliersAsync();
            List<Supplier> expectedSuppliers = randomSuppliers;

            // when
            List<Supplier> actualSuppliers = await this.apiBroker.GetAllSuppliersAsync();

            // then
            foreach (Supplier expectedSupplier in expectedSuppliers)
            {
                Supplier actualSupplier = actualSuppliers.Single(approval => approval.Id == expectedSupplier.Id);
                actualSupplier.Should().BeEquivalentTo(expectedSupplier);
                await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
            }
        }

        [Fact]
        public async Task ShouldGetSupplierAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            Supplier expectedSupplier = randomSupplier;

            // when
            Supplier actualSupplier = await this.apiBroker.GetSupplierByIdAsync(randomSupplier.Id);

            // then
            actualSupplier.Should().BeEquivalentTo(expectedSupplier);
            await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutSupplierAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            Supplier modifiedSupplier = UpdateSupplierWithRandomValues(randomSupplier);

            // when
            await this.apiBroker.PutSupplierAsync(modifiedSupplier);
            Supplier actualSupplier = await this.apiBroker.GetSupplierByIdAsync(randomSupplier.Id);

            // then
            actualSupplier.Should().BeEquivalentTo(modifiedSupplier);
            await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteSupplierAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            Supplier inputSupplier = randomSupplier;
            Supplier expectedSupplier = inputSupplier;

            // when
            Supplier deletedSupplier =
                await this.apiBroker.DeleteSupplierByIdAsync(inputSupplier.Id);

            ValueTask<Supplier> getSupplierbyIdTask =
                this.apiBroker.GetSupplierByIdAsync(inputSupplier.Id);

            // then
            deletedSupplier.Should().BeEquivalentTo(expectedSupplier);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getSupplierbyIdTask.AsTask());
        }

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
            List<Supplier> existingSuppliers = await this.apiBroker.GetAllSuppliersAsync();
            List<Supplier> randomSuppliers = await PostRandomSuppliersAsync();
            List<Supplier> expectedSuppliers = randomSuppliers;

            //When
            List<Supplier> actualSuppliers = await this.apiBroker.GetAllSuppliersOrderedDescendingAsync();

            //Then
            actualSuppliers.Count.Should().Be(expectedSuppliers.Count + existingSuppliers.Count);

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
                    randomIngestionTrackings.DeepClone().OrderBy(ingestionTracking => ingestionTracking.CreatedDate).ToList();

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

                foreach ( IngestionTracking ingestionTracking in actualSupplier.IngestionTrackings)
                {
                    await this.apiBroker.DeleteIngestionTrackingByIdAsync(ingestionTracking.Id);
                }

                await this.apiBroker.DeleteSupplierByIdAsync(actualSupplier.Id);
            }
        }
    }
}