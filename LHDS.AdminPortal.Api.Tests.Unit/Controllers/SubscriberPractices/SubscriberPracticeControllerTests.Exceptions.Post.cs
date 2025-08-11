// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
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
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();

            var addressValidationException =
                new SubscriberPracticeValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(addressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PostSubscriberPracticeAsync(someSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PostSubscriberPracticeAsync(someSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsSubscriberPracticeErrorOccurredAsync()
        {
            // given
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsSubscriberPracticeException =
                new AlreadyExistsSubscriberPracticeException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSubscriberPracticeException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsSubscriberPracticeException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedConflictObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PostSubscriberPracticeAsync(someSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidSubscriberPracticeReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberPractice someSubscriberPractice = CreateRandomSubscriberPractice();

            var alreadyExistsSubscriberPracticeException =
                new InvalidSubscriberPracticeReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSubscriberPracticeException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(addressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberPractice>(expectedBadRequestObjectResult);

            this.subscriberPracticeServiceMock.Setup(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberPractice> actualActionResult =
                await this.subscriberPracticesController.PostSubscriberPracticeAsync(someSubscriberPractice);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberPracticeServiceMock.Verify(service =>
                service.AddSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
                    Times.Once);

            this.subscriberPracticeServiceMock.VerifyNoOtherCalls();
        }
    }
}
