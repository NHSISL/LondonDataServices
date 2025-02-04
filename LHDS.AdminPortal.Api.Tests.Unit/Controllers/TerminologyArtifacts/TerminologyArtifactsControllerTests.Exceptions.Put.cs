// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using Xeptions;
using Xunit;
using RESTFulSense.Models;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();

            var TerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(TerminologyArtifactValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PutTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();

            var notFoundTerminologyArtifactException =
                new NotFoundTerminologyArtifactException(
                    terminologyArtifactId: someId);

            var TerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: someMessage,
                    innerException: notFoundTerminologyArtifactException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundTerminologyArtifactException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedNotFoundObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PutTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidTerminologyArtifactReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();

            var alreadyExistsTerminologyArtifactException =
                new InvalidTerminologyArtifactReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var TerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsTerminologyArtifactException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(TerminologyArtifactDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactDependencyValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PutTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.ModifyTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
