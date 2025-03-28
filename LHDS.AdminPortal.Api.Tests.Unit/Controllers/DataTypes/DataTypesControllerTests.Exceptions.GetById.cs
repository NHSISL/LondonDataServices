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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var DataTypeValidationException =
                new DataTypeValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataTypeValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RetrieveDataTypeByIdAsync(someId))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.GetDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RetrieveDataTypeByIdAsync(someId),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetByIdIfServerErrorOccurredAsync(Xeption serverException)
        {
            // given 
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedInternalServerErrorObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RetrieveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.GetDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RetrieveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

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
                service.RetrieveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.GetDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RetrieveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
