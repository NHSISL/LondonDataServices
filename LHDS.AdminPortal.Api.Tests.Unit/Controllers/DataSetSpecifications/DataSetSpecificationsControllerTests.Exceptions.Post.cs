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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification someDataSetSpecifications = CreateRandomDataSetSpecification();

            var DataSetSpecificationsValidationException =
                new DataSetSpecificationValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(DataSetSpecificationsValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetSpecificationsValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSetSpecifications);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DataSetSpecification someDataSet = CreateRandomDataSetSpecification();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSet);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsDataSetErrorOccurredAsync()
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
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

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetSpecificationDependencyValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidDataSetReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();

            var alreadyExistsDataSetSpecificationException =
                new InvalidDataSetSpecificationReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var DataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDataSetSpecificationException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(DataSetSpecificationDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<DataSetSpecification>(expectedBadRequestObjectResult);

            this.DataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()))
                    .ThrowsAsync(DataSetSpecificationDependencyValidationException);

            // when
            ActionResult<DataSetSpecification> actualActionResult =
                await this.DataSetSpecificationController.PostDataSetSpecificationAsync(someDataSetSpecification);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.DataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Once);

            this.DataSetSpecificationServiceMock.VerifyNoOtherCalls();
        }
    }
}
