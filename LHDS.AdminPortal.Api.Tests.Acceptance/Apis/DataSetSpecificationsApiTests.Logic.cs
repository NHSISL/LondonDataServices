// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataSetSpecifications;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using RESTFulSense.Exceptions;
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
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = inputDataSetSpecification;

            // When
            DataSetSpecification actualDataSetSpecification = await this.apiBroker.PostDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            // Cleanup
            await this.apiBroker.DeleteDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);
        }
    }
}