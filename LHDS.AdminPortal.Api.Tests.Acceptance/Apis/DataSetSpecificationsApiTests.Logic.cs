// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldPostDataSetSpecificationAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

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
        }

        [Fact]
        public async Task ShouldGetAllDataSetSpecificationsAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

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
        }

        [Fact]
        public async Task ShouldGetDataSetSpecificationByIdAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            await this.apiBroker.PostDataSetAsync(randomDataSet);

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
        }
    }
}