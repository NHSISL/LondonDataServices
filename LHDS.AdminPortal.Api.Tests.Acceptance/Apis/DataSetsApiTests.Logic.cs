// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSets
{
    public partial class DataSetsApiTests
    {
        [Fact]
        public async Task ShouldPostDataSetAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSet inputDataSet = randomDataSet;
            DataSet expectedDataSet = inputDataSet;

            // When
            DataSet actualDataSet = await this.apiBroker.PostDataSetAsync(inputDataSet);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetAllDataSetsAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            IQueryable<DataSet> randomDataSets = CreateRandomDataSets(randomSupplier.Id);
            IQueryable<DataSet> inputDataSets = randomDataSets;
            IQueryable<DataSet> expectedDataSets = inputDataSets;

            foreach (DataSet inputDataSet in inputDataSets)
            {
                await this.apiBroker.PostDataSetAsync(inputDataSet);
            }

            // When
            List<DataSet> actualDataSets = await this.apiBroker.GetAllDataSetsAsync();

            // Then
            foreach (DataSet expectedDataSet in expectedDataSets)
            {
                DataSet actualDataSet = actualDataSets.Single(approval => approval.Id == expectedDataSet.Id);

                actualDataSet.Should().BeEquivalentTo(expectedDataSet, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteDataSetByIdAsync(actualDataSet.Id);
            }

            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetDataSetByIdAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSet inputDataSet = randomDataSet;
            DataSet expectedDataSet = inputDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);

            // When
            DataSet actualDataSet =
                await this.apiBroker.GetDataSetByIdAsync(inputDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutDataSetAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSet inputDataSet = randomDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);
            DataSet modifiedDataSet = UpdateDataSetWithRandomValues(inputDataSet);

            // When
            await this.apiBroker.PutDataSetAsync(modifiedDataSet);
            DataSet actualDataSet = await this.apiBroker.GetDataSetByIdAsync(inputDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(modifiedDataSet, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteDataSetAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplier.Id);
            DataSet inputDataSet = randomDataSet;
            DataSet expectedDataSet = inputDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);

            // when
            DataSet deletedDataSet =
                await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);

            ValueTask<DataSet> getDataSetbyIdTask =
                this.apiBroker.GetDataSetByIdAsync(inputDataSet.Id);

            // then
            deletedDataSet.Should().BeEquivalentTo(expectedDataSet, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getDataSetbyIdTask.AsTask);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}