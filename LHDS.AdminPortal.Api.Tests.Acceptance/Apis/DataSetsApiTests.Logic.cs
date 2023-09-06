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
            foreach (DataSet expectedDataSet in expectedDataSets)
            {
                DataSet actualDataSet = actualDataSets.Single(approval => approval.Id == expectedDataSet.Id);
                actualDataSet.Should().BeEquivalentTo(expectedDataSet);
                await this.apiBroker.DeleteDataSetByIdAsync(actualDataSet.Id);
            }
        }

        [Fact]
        public async Task ShouldGetDataSetByIdAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet expectedDataSet = inputDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);

            // When
            DataSet actualDataSet =
                await this.apiBroker.GetDataSetByIdAsync(inputDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
        }

        [Fact]
        public async Task ShouldPutDataSetAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            await this.apiBroker.PostDataSetAsync(inputDataSet);
            DataSet modifiedDataSet = UpdateDataSetWithRandomValues(inputDataSet);

            // When
            await this.apiBroker.PutDataSetAsync(modifiedDataSet);
            DataSet actualDataSet = await this.apiBroker.GetDataSetByIdAsync(inputDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(modifiedDataSet);

            // Cleanup
            await this.apiBroker.DeleteDataSetByIdAsync(inputDataSet.Id);
        }
    }
}