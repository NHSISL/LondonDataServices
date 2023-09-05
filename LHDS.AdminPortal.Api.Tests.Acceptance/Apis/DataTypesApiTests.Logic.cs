// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.DataTypes
{
    public partial class DataTypesApiTests
    {
        [Fact]
        public async Task ShouldPostDataTypeAsync()
        {
            // Given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType expectedDataType = inputDataType;

            // When
            DataType actualDataType = await this.apiBroker.PostDataTypeAsync(inputDataType);

            // Then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            // Cleanup
            await this.apiBroker.DeleteDataTypeByIdAsync(inputDataType.Id);
        }

        [Fact]
        public async Task ShouldGetAllPdsAuditsAsync()
        {
            // Given
            IQueryable<DataType> randomDataTypes = CreateRandomDataTypes();
            IQueryable<DataType> inputDataTypes = randomDataTypes;
            IQueryable<DataType> expectedDataTypes = inputDataTypes;

            foreach (DataType inputDataType in inputDataTypes)
            {
                await this.apiBroker.PostDataTypeAsync(inputDataType);
            }

            // When
            List<DataType> actualDataTypes = await this.apiBroker.GetAllDataTypesAsync();

            // Then
            actualDataTypes.Should().BeEquivalentTo(expectedDataTypes);

            // Cleanup
            foreach (DataType inputDataType in expectedDataTypes)
            {
                await this.apiBroker.DeleteDataTypeByIdAsync(inputDataType.Id);
            }
        }
    }
}