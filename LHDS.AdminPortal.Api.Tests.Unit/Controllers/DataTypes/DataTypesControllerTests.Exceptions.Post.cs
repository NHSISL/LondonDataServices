// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataTypes
{
    public partial class DataTypesControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataType someDataType = CreateRandomDataType();

            var DataTypeValidationException =
                new DataTypeValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataTypeValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.AddDataTypeAsync(It.IsAny<DataType>()))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PostDataTypeAsync(someDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.AddDataTypeAsync(It.IsAny<DataType>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataType someDataType = CreateRandomDataType();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.AddDataTypeAsync(It.IsAny<DataType>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PostDataTypeAsync(someDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.AddDataTypeAsync(It.IsAny<DataType>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
