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
        public async Task ShouldReturnBadRequestOnPutIfValidationErrorOccurredAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            var subscriberCredentialValidationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: someMessage,
                    innerException: someInnerException);

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(subscriberCredentialValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true))
                    .ThrowsAsync(subscriberCredentialValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PutSubscriberCredentialAsync(someSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnPutIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            var notFoundSubscriberAgreementException =
                new NotFoundSubscriberAgreementException(
                    subscriberAgreementId: someId);

            var subscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: someMessage,
                    innerException: notFoundSubscriberAgreementException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundSubscriberAgreementException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedNotFoundObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true))
                    .ThrowsAsync(subscriberCredentialValidationOrchestrationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PutSubscriberCredentialAsync(someSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnFailedDependencyOnPutIfInvalidSubscriberCredentialReferenceAsync()
        {
            // given
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            var subscriberCredentialOrchestrationDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException);

            FailedDependencyObjectResult expectedBadRequestObjectResult =
                FailedDependency(subscriberCredentialOrchestrationDependencyValidationException.InnerException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true))
                    .ThrowsAsync(subscriberCredentialOrchestrationDependencyValidationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PutSubscriberCredentialAsync(someSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPutIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            SubscriberCredential someSubscriberCredential = CreateRandomSubscriberCredential();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<SubscriberCredential>(expectedBadRequestObjectResult);

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<SubscriberCredential> actualActionResult =
                await this.subscriberCredentialsController.PutSubscriberCredentialAsync(someSubscriberCredential);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.ModifyOrAddSubscriberCredentialAsync(It.IsAny<SubscriberCredential>(), false, true),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
        }
    }
}
