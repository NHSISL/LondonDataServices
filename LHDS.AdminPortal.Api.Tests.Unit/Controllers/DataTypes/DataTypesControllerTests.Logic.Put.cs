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
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType storageDataType = inputDataType.DeepClone();
            DataType expectedDataType = storageDataType.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDataType);

            var expectedActionResult =
                new ActionResult<DataType>(expectedObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.ModifyDataTypeAsync(inputDataType))
                    .ReturnsAsync(storageDataType);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PutDataTypeAsync(inputDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.ModifyDataTypeAsync(inputDataType),
                   Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
