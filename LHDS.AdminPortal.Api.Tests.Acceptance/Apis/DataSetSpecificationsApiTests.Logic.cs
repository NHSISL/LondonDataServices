// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSets;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataSetSpecifications
{
    public partial class DataSetSpecificationsApiTests
    {
        [Fact]
        public async Task ShouldGetAllDataSetSpecificationsAsync()
        {
            // Given
            IQueryable<DataSetSpecification> randomDataSetSpecifications = CreateRandomDataSetSpecifications();
            IQueryable<DataSetSpecification> inputDataSetSpecifications = randomDataSetSpecifications;
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = inputDataSetSpecifications;

            foreach (DataSetSpecification inputDataSetSpecification in inputDataSetSpecifications)
            {
                inputDataSetSpecification.DataSetId = inputDataSet.Id;
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
        }
    }
}