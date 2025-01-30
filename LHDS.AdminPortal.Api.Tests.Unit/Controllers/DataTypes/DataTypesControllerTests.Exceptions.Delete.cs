// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.DeleteDataTypeByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
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
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataTypeValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.DeleteDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedDataTypeAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedDataTypeException =
                new LockedDataTypeException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: someMessage,
                    innerException: lockedDataTypeException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(DataTypeDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(DataTypeDependencyValidationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.DeleteDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataType>(expectedBadRequestObjectResult);

            this.dataTypeServiceMock.Setup(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataType> actualActionResult =
                await this.dataTypesController.DeleteDataTypeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataTypeServiceMock.Verify(service =>
                service.RemoveDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dataTypeServiceMock.VerifyNoOtherCalls();
        }
    }
}
