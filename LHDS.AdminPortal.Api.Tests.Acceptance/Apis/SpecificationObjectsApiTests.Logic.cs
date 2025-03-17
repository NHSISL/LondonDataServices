// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SpecificationObjects
{
    public partial class SpecificationObjectsApiTests
    {
        [Fact]
        public async Task ShouldPostSpecificationObjectAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomDataSetSpecificationAsync(randomDataSet.Id);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = inputSpecificationObject;

            // When
            SpecificationObject actualSpecificationObject =
                await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(inputSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldDeleteSpecificationObjectAsync()
        {
            // given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomDataSetSpecificationAsync(randomDataSet.Id);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = inputSpecificationObject;
            await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);

            // when
            SpecificationObject deletedSpecificationObject =
                await this.apiBroker.DeleteSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            ValueTask<SpecificationObject> getSpecificationObjectbyIdTask =
                this.apiBroker.GetSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            // then
            deletedSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getSpecificationObjectbyIdTask.AsTask);
        }

        [Fact]
        public async Task ShouldGetAllSpecificationObjectsAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomDataSetSpecificationAsync(randomDataSet.Id);

            IQueryable<SpecificationObject> randomSpecificationObjects =
                CreateRandomSpecificationObjects(dataSetSpecificationId: randomDataSetSpecification.Id);

            IQueryable<SpecificationObject> inputSpecificationObjects = randomSpecificationObjects;
            IQueryable<SpecificationObject> expectedSpecificationObjects = inputSpecificationObjects;

            foreach (SpecificationObject inputSpecificationObject in inputSpecificationObjects)
            {
                await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);
            }

            // When
            List<SpecificationObject> actualSpecificationObjects =
                await this.apiBroker.GetAllSpecificationObjectsAsync();

            // Then
            foreach (SpecificationObject expectedSpecificationObject in expectedSpecificationObjects)
            {
                SpecificationObject actualSpecificationObject =
                    actualSpecificationObjects.Single(approval => approval.Id == expectedSpecificationObject.Id);

                actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));

                await this.apiBroker.DeleteSpecificationObjectByIdAsync(actualSpecificationObject.Id);
            }

            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldGetSpecificationObjectByIdAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomDataSetSpecificationAsync(randomDataSet.Id);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = inputSpecificationObject;
            await this.apiBroker.PostSpecificationObjectAsync(randomSpecificationObject);

            // When
            SpecificationObject actualSpecificationObject =
                await this.apiBroker.GetSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(inputSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }

        [Fact]
        public async Task ShouldPutSpecificationObjectAsync()
        {
            // Given
            Supplier randomSupplier = await PostRandomSupplierAsync();
            DataSet randomDataSet = await PostRandomDataSetAsync(randomSupplier.Id);

            DataSetSpecification randomDataSetSpecification =
                await PostRandomDataSetSpecificationAsync(randomDataSet.Id);

            SpecificationObject randomSpecificationObject =
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;

            SpecificationObject storageSpecificationObject = 
                await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);

            SpecificationObject modifiedSpecificationObject =
                UpdateSpecificationObjectWithRandomValues(storageSpecificationObject);

            // When
            await this.apiBroker.PutSpecificationObjectAsync(modifiedSpecificationObject);

            SpecificationObject actualSpecificationObject =
                await this.apiBroker.GetSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(modifiedSpecificationObject, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            // Cleanup
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(actualSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
            await this.apiBroker.DeleteSupplierByIdAsync(randomSupplier.Id);
        }
    }
}