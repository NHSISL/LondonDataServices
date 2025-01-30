// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.DataSetSpecifications
{
    public partial class DataSetSpecificationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification();

            var DataSetValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(dataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification();

            var notFoundDataSetSpecificationException =
                new NotFoundDataSetSpecificationException(
                    dataSetSpecificationId: someId);

            var DataSetValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: notFoundDataSetSpecificationException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDataSetSpecificationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedNotFoundObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(dataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidDataSetReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification();

            var alreadyExistsDataSetSpecificationException =
                new InvalidDataSetSpecificationReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataSetSpecificationException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(DataSetDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetDependencyValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(dataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsDataSetErrorOccurredAsync()
        {
            // given
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsDataSetSpecificationException =
                new AlreadyExistsDataSetSpecificationException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataSetSpecificationException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsDataSetSpecificationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedConflictObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetSpecificationDependencyValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(dataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataSetSpecification dataSetSpecification = CreateRandomDataSetSpecification();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.dataSetSpecificationController.PutDataSetSpecificationAsync(dataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
