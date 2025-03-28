// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType addedDataType = inputDataType.DeepClone();
            DataType expectedDataType = addedDataType.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedDataType);

            var expectedActionResult =
                new ActionResult<DataType>(expectedObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.AddDataTypeAsync(inputDataType))
                    .ReturnsAsync(addedDataType);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PostDataTypeAsync(inputDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.AddDataTypeAsync(inputDataType),
                   Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
