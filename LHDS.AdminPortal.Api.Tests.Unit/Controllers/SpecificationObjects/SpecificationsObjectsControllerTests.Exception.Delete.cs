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
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.DeleteSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

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
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.DeleteSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedSpecificationObjectAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedSpecificationObjectException =
                new LockedSpecificationObjectException(
                    message: someMessage,
                    innerException: someInnerException);

            var specificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: someMessage,
                    innerException: lockedSpecificationObjectException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(specificationObjectDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(specificationObjectDependencyValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.DeleteSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.DeleteSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RemoveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
