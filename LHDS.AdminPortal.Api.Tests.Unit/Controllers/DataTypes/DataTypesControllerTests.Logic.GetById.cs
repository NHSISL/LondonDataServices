// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Fact]
        public async Task ShouldReturnDataTypeOnGetByIdAsync()
        {
            // given 
            DataType randomDataType = CreateRandomDataType();
            Guid inputId = randomDataType.Id;
            DataType storageDataType = randomDataType.DeepClone();
            DataType expectedDataType = storageDataType.DeepClone();
            var expectedObjectResult = new OkObjectResult(expectedDataType);

            var expectedActionResult =
                new ActionResult<DataType>(expectedObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RetrieveDataTypeByIdAsync(inputId))
                    .ReturnsAsync(expectedDataType);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.GetDataTypeByIdAsync(inputId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RetrieveDataTypeByIdAsync(inputId),
                    Times.Once());

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
