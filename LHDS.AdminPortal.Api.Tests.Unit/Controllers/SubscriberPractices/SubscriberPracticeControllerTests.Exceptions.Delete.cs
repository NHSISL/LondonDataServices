// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberPractices
{
    public partial class SubscriberPracticeControllerTests
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
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.DeleteSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundSubscriberPracticeException =
                new NotFoundSubscriberPracticeException(
                    message: $"Couldn't find subscriberPractice with subscriberPracticeId: {someId}.");

            var addressValidationException =
                new SubscriberPracticeValidationException(
                    message: someMessage,
                    innerException: notFoundSubscriberPracticeException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSubscriberPracticeException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedNotFoundObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.DeleteSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedSubscriberPracticeAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedSubscriberPracticeException =
                new LockedSubscriberPracticeException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: someMessage,
                    innerException: lockedSubscriberPracticeException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(addressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.DeleteSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.DeleteSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RemoveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
