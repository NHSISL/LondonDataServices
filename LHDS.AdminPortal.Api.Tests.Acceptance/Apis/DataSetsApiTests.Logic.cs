// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ShouldGetAllDataSetsAsync()
        {
            // Given
            IQueryable<DataSet> randomDataSets = CreateRandomDataSets();
            IQueryable<DataSet> inputDataSets = randomDataSets;
            IQueryable<DataSet> expectedDataSets = inputDataSets;

            foreach (DataSet inputDataSet in inputDataSets)
            {
                await this.apiBroker.PostDataSetAsync(inputDataSet);
            }

            // When
            List<DataSet> actualDataSets = await this.apiBroker.GetAllDataSetsAsync();

            // Then
            actualDataSets.Should().BeEquivalentTo(expectedDataSets);

            // Cleanup
            foreach (DataSet inputDataSet in expectedDataSets)
            {
                await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
            }
        }
    }
}