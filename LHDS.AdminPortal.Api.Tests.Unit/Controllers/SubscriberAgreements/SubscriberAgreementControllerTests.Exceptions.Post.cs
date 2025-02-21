// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberAgreements
{
    public partial class SubscriberAgreementControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();

            var addressValidationException =
                new SubscriberAgreementValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(addressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.PostSubscriberAgreementAsync(someSubscriberAgreement);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.PostSubscriberAgreementAsync(someSubscriberAgreement);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsSubscriberAgreementErrorOccurredAsync()
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsSubscriberAgreementException =
                new AlreadyExistsSubscriberAgreementException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSubscriberAgreementException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsSubscriberAgreementException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedConflictObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.PostSubscriberAgreementAsync(someSubscriberAgreement);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPostIfInvalidSubscriberAgreementReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();

            var alreadyExistsSubscriberAgreementException =
                new InvalidSubscriberAgreementReferenceException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsSubscriberAgreementException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(addressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.PostSubscriberAgreementAsync(someSubscriberAgreement);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
