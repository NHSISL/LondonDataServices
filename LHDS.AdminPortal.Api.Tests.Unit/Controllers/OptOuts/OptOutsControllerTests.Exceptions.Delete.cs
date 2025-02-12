// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.OptOuts
{
    public partial class OptOutsControllerTests
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
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(someId))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.DeleteOptOutByIdAsync(someId);

            // then
            actualActionResult.Should().BeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(someId),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundOptOutException =
                new NotFoundOptOutException(message: $"Couldn't find optOut with optOutId: {someId}.");

            var OptOutValidationException =
                new OptOutProcessingValidationException(
                    message: someMessage,
                    innerException: notFoundOptOutException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundOptOutException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedNotFoundObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(someId))
                    .ThrowsAsync(OptOutValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.DeleteOptOutByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(someId),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedOptOutAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedOptOutException =
                new LockedOptOutException(
                    message: someMessage,
                    innerException: someInnerException);

            var OptOutDependencyValidationException =
                new OptOutProcessingDependencyValidationException(
                    message: someMessage,
                    innerException: lockedOptOutException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(OptOutDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(someId))
                    .ThrowsAsync(OptOutDependencyValidationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.DeleteOptOutByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(someId),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<OptOut>(expectedBadRequestObjectResult);

            this.optOutProcessingServiceMock.Setup(service =>
                service.RemoveOptOutByIdAsync(someId))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<OptOut> actualActionResult =
                await this.optOutsController.DeleteOptOutByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.optOutProcessingServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(someId),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
