// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
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
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.DeleteSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundSubscriberAgreementException =
                new NotFoundSubscriberAgreementException(
                    subscriberAgreementId: someId);

            var addressValidationException =
                new SubscriberAgreementValidationException(
                    message: someMessage,
                    innerException: notFoundSubscriberAgreementException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSubscriberAgreementException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedNotFoundObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.DeleteSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfLockedSubscriberAgreementAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var lockedSubscriberAgreementException =
                new LockedSubscriberAgreementException(
                    message: someMessage,
                    innerException: someInnerException);

            var addressDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: someMessage,
                    innerException: lockedSubscriberAgreementException);

            LockedObjectResult expectedBadRequestObjectResult =
                Locked(addressDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressDependencyValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.DeleteSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.DeleteSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
