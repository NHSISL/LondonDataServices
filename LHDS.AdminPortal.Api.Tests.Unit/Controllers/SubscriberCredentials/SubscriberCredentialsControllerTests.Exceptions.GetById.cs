// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.SubscriberCredentials
{
    public partial class SubscriberCredentialsControllerTests
    {
        [Fact]
        public async Task ShouldReturnBadRequestOnGetByIdIfValidationErrorOccurredAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var subscriberCredentialValidationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(subscriberCredentialValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(someId, true))
                    .ThrowsAsync(subscriberCredentialValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.GetSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(someId, true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberCredential>(expectedInternalServerErrorObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), true))
                    .ThrowsAsync(serverException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.GetSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), true),
                    Times.Once());

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnGetByIdIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundSubscriberCredentialException =
                new NotFoundSubscriberAgreementException(
                    subscriberAgreementId: someId);

            var subscriberCredentialValidationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: someMessage,
                    innerException: notFoundSubscriberCredentialException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSubscriberCredentialException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedNotFoundObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), true))
                    .ThrowsAsync(subscriberCredentialValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.GetSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(It.IsAny<Guid>(), true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
