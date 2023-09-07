// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.Core.Extensions.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    public partial class DataSetSpecificationsApiTests
    {
        [Fact]
        public async Task ShouldPostDataSetSpecificationAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            inputDataSetSpecification.DataSetId = inputDataSet.Id;
            DataSetSpecification expectedDataSetSpecification = inputDataSetSpecification;

            // When
            DataSetSpecification actualDataSetSpecification =
                await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            // Cleanup
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
        }
    }
}