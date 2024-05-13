// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using RESTFulSense.Exceptions;
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
        public async Task ShouldGetAllDataTypesAsync()
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
            foreach (DataType expectedDataType in expectedDataTypes)
            {
                DataType actualDataType = actualDataTypes.Single(approval => approval.Id == expectedDataType.Id);
                actualDataType.Should().BeEquivalentTo(expectedDataType);
                await this.apiBroker.DeleteDataTypeByIdAsync(actualDataType.Id);
            }
        }

        [Fact]
        public async Task ShouldGetDataTypeByIdAsync()
        {
            // Given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType expectedDataType = inputDataType;
            await this.apiBroker.PostDataTypeAsync(inputDataType);

            // When
            DataType actualDataType =
                await this.apiBroker.GetDataTypeByIdAsync(inputDataType.Id);

            // Then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            // Cleanup
            await this.apiBroker.DeleteDataTypeByIdAsync(inputDataType.Id);
        }

        [Fact]
        public async Task ShouldPutDataTypeAsync()
        {
            // Given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            await this.apiBroker.PostDataTypeAsync(inputDataType);
            DataType modifiedDataType = UpdateDataTypeWithRandomValues(inputDataType);

            // When
            await this.apiBroker.PutDataTypeAsync(modifiedDataType);
            DataType actualDataType = await this.apiBroker.GetDataTypeByIdAsync(inputDataType.Id);

            // Then
            actualDataType.Should().BeEquivalentTo(modifiedDataType);

            // Cleanup
            await this.apiBroker.DeleteDataTypeByIdAsync(inputDataType.Id);
        }

        [Fact]
        public async Task ShouldDeleteDataTypeAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType expectedDataType = inputDataType;
            await this.apiBroker.PostDataTypeAsync(inputDataType);

            // when
            DataType deletedDataType =
                await this.apiBroker.DeleteDataTypeByIdAsync(inputDataType.Id);

            ValueTask<DataType> getDataTypebyIdTask =
                this.apiBroker.GetDataTypeByIdAsync(inputDataType.Id);

            // then
            deletedDataType.Should().BeEquivalentTo(expectedDataType);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getDataTypebyIdTask.AsTask);
        }
    }
}