// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
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

            DataSetSpecification randomDataSetSpecification
                = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject
                = CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

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
        public async Task ShouldPutSpecificationObjectAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            DataSetSpecification randomDataSetSpecification
               = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject
               = CreateRandomSpecificationObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            await this.apiBroker.PostSpecificationObjectAsync(inputSpecificationObject);

            SpecificationObject modifiedSpecificationObject =
                UpdateSpecificationObjectWithRandomValues(inputSpecificationObject);

            // When
            await this.apiBroker.PutSpecificationObjectAsync(modifiedSpecificationObject);

            SpecificationObject actualSpecificationObject =
                await this.apiBroker.GetSpecificationObjectByIdAsync(inputSpecificationObject.Id);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(modifiedSpecificationObject);

            // Cleanup
            await this.apiBroker.DeleteSpecificationObjectByIdAsync(actualSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
        }
    }
}