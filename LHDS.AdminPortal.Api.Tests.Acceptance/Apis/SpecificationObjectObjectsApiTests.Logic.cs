// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
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
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            DataSetSpecification randomDataSetSpecification = 
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject = 
                CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = inputSpecificationObject;

            // When
            SpecificationObject actualSpecificationObject =
                await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            // Cleanup
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(inputSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
        }

        [Fact]
        public async Task ShouldDeleteSpecificationObjectAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            DataSetSpecification randomDataSetSpecification = 
                CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

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
            deletedSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getSpecificationObjectbyIdTask.AsTask());
        }
    }
}