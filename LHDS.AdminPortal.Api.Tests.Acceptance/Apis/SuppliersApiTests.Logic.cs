// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
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
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getSupplierbyIdTask.AsTask);
        }
    }
}