// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SpecificationObjects;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetObjects
{
    public partial class DataSetObjectsApiTests
    {
        [Fact]
        public async Task ShouldPostDataSetObjectAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

            DataSetSpecification randomDataSetSpecification
                = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);

            await this.apiBroker.PostDataSetSpecificationAsync(randomDataSetSpecification);

            SpecificationObject randomSpecificationObject
                = CreateRandomDataSetObject(dataSetSpecificationId: randomDataSetSpecification.Id);

            SpecificationObject inputSpecificationObject = randomSpecificationObject;
            SpecificationObject expectedSpecificationObject = inputSpecificationObject;

            // When
            SpecificationObject actualSpecificationObject =
                await this.apiBroker.PostDataSetObjectAsync(inputSpecificationObject);

            // Then
            actualSpecificationObject.Should().BeEquivalentTo(expectedSpecificationObject);

            // Cleanup
            await this.apiBroker.DeleteDataSetObjectByIdAsync(inputSpecificationObject.Id);
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(randomDataSet.Id);
        }
    }
}