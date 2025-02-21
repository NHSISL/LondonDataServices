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
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var addressValidationException =
                new SubscriberAgreementValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(addressValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberAgreement>(expectedBadRequestObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(someId))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.GetSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(someId),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberAgreement>(expectedInternalServerErrorObjectResult);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.GetSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
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
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(addressValidationException);

            // when
            ActionResult<SubscriberAgreement> actualActionResult =
                await this.subscriberAgreementsController.GetSubscriberAgreementByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
        }
    }
}
