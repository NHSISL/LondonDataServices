// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SpecificationObjects
{
    public partial class SpecificationObjectsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            var specificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(specificationObjectValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PostSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PostSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsSpecificationObjectErrorOccurredAsync()
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsSpecificationObjectException =
                new AlreadyExistsSpecificationObjectException(
                    message: someMessage,
                    innerException: someInnerException);

            var specificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSpecificationObjectException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsSpecificationObjectException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedConflictObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectDependencyValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PostSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidSpecificationObjectReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            var alreadyExistsSpecificationObjectException =
                new InvalidSpecificationObjectReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var specificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSpecificationObjectException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(specificationObjectDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectDependencyValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PostSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.AddSpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
