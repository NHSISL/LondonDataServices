// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.DataType;
using Microsoft.AspNetCore.Mvc;
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
    }
}