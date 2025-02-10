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
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.DeleteSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
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
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(subscriberCredentialValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.DeleteSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnDeleteIDependencyExceptionOnSubscriberCredentialAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            var subscriberCredentialOrchestrationDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(subscriberCredentialOrchestrationDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(subscriberCredentialOrchestrationDependencyValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.DeleteSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
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
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.DeleteSubscriberCredentialByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RemoveSubscriberCredentialByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
