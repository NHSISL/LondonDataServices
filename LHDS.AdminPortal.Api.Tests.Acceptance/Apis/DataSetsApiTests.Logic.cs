// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSets
{
    public partial class DataSetsApiTests
    {
        [Fact]
        public async Task ShouldPostDataSetAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet expectedDataSet = inputDataSet;

            // When
            DataSet actualDataSet = await this.apiBroker.PostDataSetAsync(inputDataSet);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
        }
    }
}