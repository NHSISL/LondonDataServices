// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    public partial class DataSetSpecificationsApiTests
    {
        [Fact]
        public async Task ShouldPostDataSetSpecificationAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification
                = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = inputDataSetSpecification;

            // When
            DataSetSpecification actualDataSetSpecification =
                await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            // Cleanup
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact(Skip = "Will fix in another PR.")]
        public async Task ShouldGetAllDataSetSpecificationsAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            IQueryable<DataSetSpecification> randomDataSetSpecifications =
                CreateRandomDataSetSpecifications(dataSetId: randomDataSet.Id);

            IQueryable<DataSetSpecification> inputDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = inputDataSetSpecifications;

            foreach (DataSetSpecification inputDataSetSpecification in inputDataSetSpecifications)
            {
                await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);
            }

            // When
            List<DataSetSpecification> actualDataSetSpecifications =
                await this.apiBroker.GetAllDataSetSpecificationsAsync();

            // Then
            foreach (DataSetSpecification expectedDataSetSpecification in expectedDataSetSpecifications)
            {
                DataSetSpecification actualDataSetSpecification =
                    actualDataSetSpecifications.Single(approval => approval.Id == expectedDataSetSpecification.Id);

                actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);
                await this.apiBroker.DeleteDataSetSpecificationByIdAsync(actualDataSetSpecification.Id);
            }

            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetDataSetSpecificationByIdAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = inputDataSetSpecification;
            await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // When
            DataSetSpecification actualDataSetSpecification =
                await this.apiBroker.GetDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            // Cleanup
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutDataSetSpecificationAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification
               = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            DataSetSpecification modifiedDataSetSpecification =
                UpdateDataSetSpecificationWithRandomValues(inputDataSetSpecification);

            // When
            await this.apiBroker.PutDataSetSpecificationAsync(modifiedDataSetSpecification);

            DataSetSpecification actualDataSetSpecification =
                await this.apiBroker.GetDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(modifiedDataSetSpecification);

            // Cleanup
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(actualDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteDataSetSpecificationAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = inputDataSetSpecification;
            await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // when
            DataSetSpecification deletedDataSetSpecification =
                await this.apiBroker.DeleteDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);

            ValueTask<DataSetSpecification> getDataSetSpecificationbyIdTask =
                this.apiBroker.GetDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);

            // then
            deletedDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getDataSetSpecificationbyIdTask.AsTask);
        }
    }
}