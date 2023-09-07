// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    public partial class DataSetSpecificationsApiTests
    {
        [Fact]
        public async Task ShouldGetDataSetSpecificationByIdAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(dataSetId: randomDataSet.Id);
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
        }
    }
}