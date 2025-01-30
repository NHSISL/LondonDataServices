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
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
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
                service.ModifyDataTypeAsync(It.IsAny<DataType>()))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PutDataTypeAsync(someDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.ModifyDataTypeAsync(It.IsAny<DataType>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            DataType someDataType = CreateRandomDataType();

            var notFoundDataTypeException =
                new NotFoundDataTypeException(
                    dataTypeId: someId);

            var DataTypeValidationException =
                new DataTypeValidationException(
                    message: someMessage,
                    innerException: notFoundDataTypeException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDataTypeException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedNotFoundObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.ModifyDataTypeAsync(It.IsAny<DataType>()))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PutDataTypeAsync(someDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.ModifyDataTypeAsync(It.IsAny<DataType>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidDataTypeReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataType someDataType = CreateRandomDataType();

            var alreadyExistsDataTypeException =
                new InvalidDataTypeReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataTypeException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(DataTypeDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.ModifyDataTypeAsync(It.IsAny<DataType>()))
                    .ThrowsAsync(DataTypeDependencyValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.PutDataTypeAsync(someDataType);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.ModifyDataTypeAsync(It.IsAny<DataType>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
