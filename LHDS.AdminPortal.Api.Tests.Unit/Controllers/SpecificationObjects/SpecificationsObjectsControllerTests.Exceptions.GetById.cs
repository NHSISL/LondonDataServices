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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var specificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(specificationObjectValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SpecificationObject>(expectedBadRequestObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectByIdAsync(someId))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.GetSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectByIdAsync(someId),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SpecificationObject>(expectedInternalServerErrorObjectResult);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.GetSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(specificationObjectValidationException);

            // when
            ActionResult<SpecificationObject> actualActionResult =
                await this.specificationObjectsController.GetSpecificationObjectByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
        }
    }
}
