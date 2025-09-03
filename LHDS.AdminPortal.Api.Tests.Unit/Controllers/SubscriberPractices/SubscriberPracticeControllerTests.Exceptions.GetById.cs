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
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var addressValidationException =
                new SubscriberPracticeValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(addressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RetrieveSubscriberPracticeByIdAsync(someId))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.GetSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RetrieveSubscriberPracticeByIdAsync(someId),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberPractice>(expectedInternalServerErrorObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.RetrieveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.GetSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RetrieveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveSubscriberPracticeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.GetSubscriberPracticeByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.RetrieveSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
