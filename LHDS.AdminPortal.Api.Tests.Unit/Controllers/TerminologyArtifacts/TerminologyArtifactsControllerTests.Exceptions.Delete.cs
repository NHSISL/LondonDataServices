// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException ?? validationException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

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
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(TerminologyArtifactValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedTerminologyArtifactAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedTerminologyArtifactException =
                new LockedTerminologyArtifactException(
                    message: someMessage,
                    innerException: someInnerException);

            var TerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: someMessage,
                    innerException: lockedTerminologyArtifactException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(TerminologyArtifactDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(TerminologyArtifactDependencyValidationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<TerminologyArtifact>(expectedBadRequestObjectResult);

            this.terminologyArtifactsServiceMock.Setup(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<TerminologyArtifact> actualActionResult =
                await this.terminologyArtifactsController.DeleteTerminologyArtifactByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.terminologyArtifactsServiceMock.Verify(service =>
                service.RemoveTerminologyArtifactByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.terminologyArtifactsServiceMock.VerifyNoOtherCalls();
        }
    }    
}