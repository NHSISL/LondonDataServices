// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.TerminologyArtifacts
{
    public partial class TerminologyArtifactsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
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
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PostTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PostTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsTerminologyArtifactErrorOccurredAsync()
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsTerminologyArtifactException =
                new AlreadyExistsTerminologyArtifactException(
                    message: someMessage,
                    innerException: someInnerException);

            var TerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsTerminologyArtifactException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsTerminologyArtifactException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedConflictObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactDependencyValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PostTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidTerminologyArtifactReferenceAsync()
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
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()))
                    .ThrowsAsync(TerminologyArtifactDependencyValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.PostTerminologyArtifactAsync(someTerminologyArtifact);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.AddTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }
}
