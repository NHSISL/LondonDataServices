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
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
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
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            var notFoundSpecificationObjectException =
                new NotFoundSpecificationObjectException(
                    specificationObjectId: someId);

            var specificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: someMessage,
                    innerException: notFoundSpecificationObjectException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSpecificationObjectException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedNotFoundObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidSpecificationObjectReferenceAsync()
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
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectDependencyValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPutIfAlreadyExistsSpecificationObjectErrorOccurredAsync()
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
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(specificationObjectDependencyValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            SpecificationObject someSpecificationObject = CreateRandomSpecificationObject();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.PutSpecificationObjectAsync(someSpecificationObject);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.ModifySpecificationObjectAsync(It.IsAny<SpecificationObject>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
